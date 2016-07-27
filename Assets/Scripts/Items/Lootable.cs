using UnityEngine;

public class Lootable : MonoBehaviour {

	private Spawner source;

	public void SetSource (Spawner source) {
		this.source = source;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			PerformLoot();
		}
	}

	void PerformLoot () {
		if (source != null) {
			source.OnItemPickup(this);
		}
		Destroy(gameObject);
	}
}
