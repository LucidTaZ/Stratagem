using UnityEngine;

public class Lootable : MonoBehaviour, Spawnable {

	private Spawner source;

	public void SetSource (Spawner source) {
		this.source = source;
	}

	void OnTriggerEnter (Collider other) {
		Inventory inventory;
		if ((inventory = other.gameObject.GetComponent<Inventory>()) != null) {
			PerformLoot(inventory);
		}
	}

	void PerformLoot (Inventory targetInventory) {
		if (source != null) {
			source.OnSubjectDestroyed(gameObject);
		}
		Destroy(gameObject);
	}
}
