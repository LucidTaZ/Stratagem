using UnityEngine;

public class Lootable : MonoBehaviour, Spawnable {

	private Spawner source;

	public void SetSource (Spawner source) {
		this.source = source;
	}

	void OnTriggerEnter (Collider other) {
		Inventory inventory;
		if ((inventory = other.GetComponent<Inventory>()) != null) {
			PerformLoot(inventory, other.gameObject);
		}
	}

	void PerformLoot (Inventory inventory, GameObject other) {
		BelongsToTeam btt = other.GetComponent<BelongsToTeam>();
		if (btt != null) {
			Debug.Log("TODO: Item looted for team " + btt.team.name);
		}

		Debug.Log("TODO: Item transfer to other inventory");

		if (source != null) {
			source.OnSubjectDestroyed(gameObject);
		}
		Destroy(gameObject);
	}
}
