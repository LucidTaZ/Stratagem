using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCollection {
	public List<Item> Items;

	public List<GameObject> InstantiateGroundItems () {
		// Returns all items
		List<GameObject> result = new List<GameObject>();
		foreach (Item item in Items) {
			result.Add(item.InstantiateGroundItem());
		}
		return result;
	}
}
