using UnityEngine;

public class Lootable : MonoBehaviour {
	public ItemIdentifier ItemToLoot;

	void Start () {
		Debug.Assert(ItemToLoot != null);
	}

	void OnTriggerEnter (Collider other) {
		Inventory inventory;
		if ((inventory = other.GetComponent<Inventory>()) != null) {
			PerformLoot(inventory, other.gameObject);
		}
	}

	void PerformLoot (Inventory inventory, GameObject other) {
		Item item = ItemFactory.Instance().Create(ItemToLoot);
		inventory.Add(item);

		Destroy(gameObject);
	}
}
