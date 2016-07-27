using UnityEngine;

public class Unit : MonoBehaviour, Spawnable {
	Spawner spawner;

	public void SetSource (Spawner source) {
		spawner = source;
	}

	void OnDestroy () {
		if (spawner != null) {
			spawner.OnSubjectDestroyed(gameObject);
		}
	}
}
