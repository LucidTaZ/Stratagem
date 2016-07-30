using UnityEngine;

public class Spawnable : MonoBehaviour {
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
