using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Inventory : NetworkBehaviour {
	public List<Item> Contents = new List<Item>();

	[ClientRpc]
	public void RpcAdd (ItemIdentifier itemIdentifier) {
		// We take the identifier and not the item object itself, because it loses polymorphism during transport
		Item item = ItemFactory.Instance().Create(itemIdentifier);
		Add(item);
	}

	public void Add (Item item) {
		Contents.Add(item);
	}

	public void Remove (Item item) {
		if (!Contents.Remove(item)) {
			Debug.LogError("Failed to remove item from inventory");
		}
	}

	public void Remove (ItemIdentifier itemIdentifier, int count = 1) {
		for (int i = 0; i < count; i++) {
			Take(itemIdentifier);
		}
	}

	public Item Take (ItemIdentifier itemIdentifier) {
		foreach (Item item in Contents) {
			if (item.Name == itemIdentifier.Name) {
				Remove(item);
				return item;
			}
		}
		Debug.LogError("Tried to take item but it is not there");
		return null;
	}

	public bool Contains (ItemIdentifier itemIdentifier) {
		foreach (Item item in Contents) {
			if (item.Name == itemIdentifier.Name) {
				return true;
			}
		}
		return false;
	}

	public int Count (ItemIdentifier itemIdentifier) {
		int result = 0;
		foreach (Item item in Contents) {
			if (item.Name == itemIdentifier.Name) {
				result++;
			}
		}
		return result;
	}
}
