using UnityEngine;

public class Inventory : MonoBehaviour {
	public ItemCollection Contents = new ItemCollection();

	public void Add (Item item) {
		Contents.Items.Add(item);
	}

	public void Remove (Item item) {
		if (!Contents.Items.Remove(item)) {
			Debug.LogError("Failed to remove item from inventory");
		}
	}
}
