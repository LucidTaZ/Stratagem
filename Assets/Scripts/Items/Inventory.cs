using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class Inventory : NetworkBehaviour {
	[Serializable]
	public struct ItemStack {
		public int Count;
		public ItemIdentifier ItemId;
	}

	// http://forum.unity3d.com/threads/syncvar-vs-clientrpc-for-syncing-arrays-and-structs.350203/
	public class ItemCollection : SyncListStruct<ItemStack> {}

	ItemCollection Contents = new ItemCollection();

	public int Capacity = int.MaxValue;

	List<Action> mutationCallbacks = new List<Action>();

	public bool HasSpace (int delta = 1) {
		return Capacity >= Count() + delta;
	}

	public void Add (Item item) {
		if (Count() >= Capacity) {
			Debug.LogError("Inventory full, use HasSpace() before trying to add! Item not added.");
			return;
		}
		ItemIdentifier itemIdentifier = new ItemIdentifier(item.Name);
		if (Contains(itemIdentifier)) {
			for (int i = 0; i < Contents.Count; i++) {
				if (Contents[i].ItemId.Equals(itemIdentifier)) {
					ItemStack its = Contents[i]; // Cannot directly modify the list
					its.Count++;
					Contents[i] = its;
				}
			}
		} else {
			ItemStack its = new ItemStack();
			its.Count = 1;
			its.ItemId = itemIdentifier;
			Contents.Add(its);
		}
		sendMutationCallbacks();
	}

	public void Remove (Item item) {
		Remove(new ItemIdentifier(item.Name));
	}

	public void Remove (ItemIdentifier itemIdentifier, int count = 1) {
		for (int i = 0; i < count; i++) {
			Take(itemIdentifier);
		}
		sendMutationCallbacks();
	}

	public Item Take (ItemIdentifier itemIdentifier) {
		for (int i = 0; i < Contents.Count; i++) {
			if (Contents[i].ItemId.Equals(itemIdentifier)) {
				Item item = ItemFactory.Instance().Create(Contents[i].ItemId);
				if (Contents[i].Count > 1) {
					ItemStack its = Contents[i]; // Cannot directly modify the list
					its.Count--;
					Contents[i] = its;;
				} else {
					Contents.RemoveAt(i);
				}
				sendMutationCallbacks();
				return item;
			}
		}

		Debug.LogError("Item not found");
		return null;
	}

	public bool Contains (ItemIdentifier itemIdentifier) {
		foreach (ItemStack its in Contents) {
			if (its.ItemId.Equals(itemIdentifier)) {
				return true;
			}
		}
		return false;
	}

	public int Count () {
		int sum = 0;
		foreach (ItemStack its in Contents) {
			sum += its.Count;
		}
		return sum;
	}

	public bool IsEmpty () {
		return Contents.Count == 0;
	}

	public int Count (ItemIdentifier itemIdentifier) {
		int sum = 0;
		foreach (ItemStack its in Contents) {
			if (its.ItemId.Equals(itemIdentifier)) {
				sum += its.Count;
			}
		}
		return sum;
	}

	public List<Item> GetContents () {
		List<Item> result = new List<Item>();
		foreach (ItemStack its in Contents) {
			for (int i = 0; i < its.Count; i++) {
				Item item = ItemFactory.Instance().Create(its.ItemId);
				result.Add(item);
			}
		}
		return result;
	}

	public List<Item.Stack> GetStackedContents () {
		List<Item.Stack> result = new List<Item.Stack>();
		foreach (ItemStack its in Contents) {
			Item.Stack element = new Item.Stack();
			element.ContainedItem = ItemFactory.Instance().Create(its.ItemId);
			element.Count = its.Count;
			result.Add(element);
		}
		return result;
	}

	public void AddMutationCallback (Action callback) {
		mutationCallbacks.Add(callback);
	}

	void sendMutationCallbacks () {
		if (NetworkServer.active) {
			RpcSendMutationCallbacks();
		} else {
			// The mutation originated from the client-side. This means the player interacted with his own inventory, for which the callbacks only matter locally anyway.
			// If that ever changes, this must be sent in a Command to the server, which calls the Rpc again on all clients
			foreach (Action callback in mutationCallbacks) {
				callback.Invoke();
			}
		}
	}

	[ClientRpc]
	void RpcSendMutationCallbacks () {
		foreach (Action callback in mutationCallbacks) {
			callback.Invoke();
		}
	}
}
