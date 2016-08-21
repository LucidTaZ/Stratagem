using UnityEngine;
using UnityEngine.Networking;

public class Lootable : NetworkBehaviour {
	public ItemIdentifier ItemToLoot;

	void OnTriggerEnter (Collider other) {
		if (!hasAuthority) {
			return;
		}
		Inventory inventory;
		if ((inventory = other.GetComponent<Inventory>()) != null) {
			PerformLoot(inventory);
		}
	}

	void PerformLoot (Inventory inventory) {
		Item item = ItemFactory.Instance().Create(ItemToLoot);
		inventory.Add(item);
		NetworkServer.Destroy(gameObject);
	}
}
