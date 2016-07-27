using UnityEngine;

public class DestroyOnImpact : MonoBehaviour {
	void OnCollisionEnter (Collision collision) {
		Destroy(gameObject);
	}
}
