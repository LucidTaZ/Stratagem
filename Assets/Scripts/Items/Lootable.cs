using UnityEngine;
using UnityEngine.Networking;

public class Lootable : NetworkBehaviour {
	public ItemIdentifier ItemToLoot;

	void Start () {
		Debug.Assert(ItemToLoot != null);
	}

	void OnTriggerEnter (Collider other) {
		if (!hasAuthority) {
			return;
		}
		Inventory inventory;
		if ((inventory = other.GetComponent<Inventory>()) != null) {
			PerformLoot(inventory, other.gameObject);
		}
	}

	void PerformLoot (Inventory inventory, GameObject other) {
		Item item = ItemFactory.Instance().Create(ItemToLoot);
		inventory.RpcAdd(item);

		NetworkServer.Destroy(gameObject);
	}
}
