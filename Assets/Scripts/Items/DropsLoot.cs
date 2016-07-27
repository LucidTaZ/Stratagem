using UnityEngine;

public class DropsLoot : MonoBehaviour {

	public GameObject Loot;

	bool quitting = false;

	void OnApplicationQuit () {
		quitting = true;
	}

	void OnDestroy () {
		// Also gets called on application quit.
		if (!quitting) {
			dropLoot();
		}
	}

	void dropLoot () {
		GameObject loot = GameObject.Instantiate(Loot);
		loot.transform.position = transform.position;
	}
}
