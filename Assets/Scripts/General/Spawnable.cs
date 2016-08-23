using UnityEngine;

public class Spawnable : MonoBehaviour {
	Spawner spawner;

	PlayerSpawner heart;

	public void SetSource (Spawner source) {
		spawner = source;
	}

	public void SetHeart (PlayerSpawner source) {
		heart = source;
	}

	void OnDestroy () {
		if (spawner != null) {
			spawner.OnSubjectDestroyed(gameObject);
		}
		if (heart != null) {
			heart.OnSubjectDestroyed(gameObject);
		}
	}
}
