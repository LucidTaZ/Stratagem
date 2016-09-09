using UnityEngine;
using UnityEngine.Networking;

public class Lootable : NetworkBehaviour {
	public ItemIdentifier ItemToLoot;

	void OnTriggerEnter (Collider other) {
		if (!hasAuthority) {
			return;
		}
		Inventory inventory = other.GetComponent<Inventory>();
		if (inventory != null && inventory.HasSpace()) {
			PerformLoot(inventory);
		}
	}

	void PerformLoot (Inventory inventory) {
		Item item = ItemFactory.Instance().Create(ItemToLoot);
		inventory.Add(item);
		NetworkServer.Destroy(gameObject);
	}
}
