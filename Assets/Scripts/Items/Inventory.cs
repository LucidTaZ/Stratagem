using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour {
	public ItemCollection Contents = new ItemCollection();

	[ClientRpc]
	public void RpcAdd (Item item) {
		Add(item);
	}

	public void Add (Item item) {
		Contents.Items.Add(item);
	}

	public void Remove (Item item) {
		if (!Contents.Items.Remove(item)) {
			Debug.LogError("Failed to remove item from inventory");
		}
	}

	public void Remove (ItemIdentifier itemIdentifier, int count = 1) {
		for (int i = 0; i < count; i++) {
			Take(itemIdentifier);
		}
	}

	public Item Take (ItemIdentifier itemIdentifier) {
		foreach (Item item in Contents.Items) {
			if (item.Name == itemIdentifier.Name) {
				Remove(item);
				return item;
			}
		}
		Debug.LogError("Tried to take item but it is not there");
		return null;
	}

	public bool Contains (ItemIdentifier itemIdentifier) {
		foreach (Item item in Contents.Items) {
			if (item.Name == itemIdentifier.Name) {
				return true;
			}
		}
		return false;
	}

	public int Count (ItemIdentifier itemIdentifier) {
		int result = 0;
		foreach (Item item in Contents.Items) {
			if (item.Name == itemIdentifier.Name) {
				result++;
			}
		}
		return result;
	}
}
