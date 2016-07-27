using UnityEngine;
using System.Collections.Generic;

public class Lootsplosion : MonoBehaviour {

	public ItemCollection itemCollection;

	public int DespawnSeconds = 0;

	const float FORCE = 10;
	const float RADIUS = 2;
	const float UPWARDS_MODIFIER = 0.2f;

	void Start () {
		List<GameObject> items = itemCollection.InstantiateGroundItems();
		foreach (GameObject item in items) {
			setupSpawnedItem(item);
		}
		Destroy(gameObject);
	}

	void setupSpawnedItem (GameObject item) {
		item.transform.rotation = getSpawnRotation();
		item.transform.position = transform.position + getSpawnLocation();
		item.GetComponent<Rigidbody>().AddExplosionForce(FORCE, transform.position, RADIUS, UPWARDS_MODIFIER, ForceMode.VelocityChange);

		if (DespawnSeconds > 0) {
			Despawn despawn = item.AddComponent<Despawn>();
			despawn.TimeToLive = DespawnSeconds;
		}
	}

	Quaternion getSpawnRotation () {
		return Quaternion.AngleAxis(Random.Range(0, 359), Vector3.up);
	}

	Vector3 getSpawnLocation () {
		Vector3 result = Random.insideUnitSphere;
		if (result.y < 0) {
			result *= -1; // Make sure we are above the explosion
		}
		return result;
	}
}
