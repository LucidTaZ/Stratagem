using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Inventory : NetworkBehaviour {
	// http://forum.unity3d.com/threads/syncvar-vs-clientrpc-for-syncing-arrays-and-structs.350203/
	public class ItemCollection : SyncListStruct<ItemIdentifier> {}

	public ItemCollection Contents = new ItemCollection();

	public void Add (Item item) {
		Contents.Add(new ItemIdentifier(item.Name));
	}

	public void Remove (Item item) {
		if (!Contents.Remove(new ItemIdentifier(item.Name))) {
			Debug.LogError("Failed to remove item from inventory");
		}
	}

	public void Remove (ItemIdentifier itemIdentifier, int count = 1) {
		for (int i = 0; i < count; i++) {
			Take(itemIdentifier);
		}
	}

	public Item Take (ItemIdentifier itemIdentifier) {
		foreach (ItemIdentifier ii in Contents) {
			if (ii.Equals(itemIdentifier)) {
				Item item = ItemFactory.Instance().Create(ii);
				Remove(item);
				return item;
			}
		}
		Debug.LogError("Tried to take item but it is not there");
		return null;
	}

	public bool Contains (ItemIdentifier itemIdentifier) {
		foreach (ItemIdentifier ii in Contents) {
			if (ii.Equals(itemIdentifier)) {
				return true;
			}
		}
		return false;
	}

	public int Count (ItemIdentifier itemIdentifier) {
		int result = 0;
		foreach (ItemIdentifier ii in Contents) {
			if (ii.Equals(itemIdentifier)) {
				result++;
			}
		}
		return result;
	}

	public List<Item> GetContents () {
		List<Item> result = new List<Item>();
		foreach (ItemIdentifier ii in Contents) {
			Item item = ItemFactory.Instance().Create(ii);
			result.Add(item);
		}
		return result;
	}
}
