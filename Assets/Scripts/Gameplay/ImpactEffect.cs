using UnityEngine;

public class ImpactEffect : MonoBehaviour {
	public GameObject EffectToSpawn;

	void OnCollisionEnter (Collision collision) {
		ContactPoint hitpoint = collision.contacts[0];
		createEffect(hitpoint.point, hitpoint.normal, collision.transform);
		Destroy(this);
	}

	void OnTriggerEnter (Collider other) {
		// We need this method because units have triggers, not colliders
		if (other.GetComponent<Hitpoints>() != null) {
			Rigidbody rb = GetComponent<Rigidbody>();
			Vector3 hitpoint = other.ClosestPointOnBounds(transform.position);
			createEffect(hitpoint, -rb.velocity.normalized, other.transform);
			Destroy(this);
		}
	}

	void createEffect (Vector3 position, Vector3 normal, Transform parent) {
		GameObject instance = Instantiate(EffectToSpawn);
		instance.transform.position = position;
		instance.transform.rotation = Quaternion.LookRotation(-normal);

		// Move with the hit object and disappear when it disappears
		instance.transform.parent = parent;

		// Correct the scale back to "normal" size, regardless of the parent object scale
		instance.transform.localScale = new Vector3(
			1.0f / parent.localScale.x,
			1.0f / parent.localScale.y,
			1.0f / parent.localScale.z
		);
	}
}
