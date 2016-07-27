using UnityEngine;

public class CollectResources : MonoBehaviour, Spawnable {

	public float Range = Mathf.Infinity;
	public GameObject currentTarget;

	GameObject spawner;
	Vector3 basePosition;

	bool currentlyTracking; // When false, found no resource, so head home

	void Start () {
		basePosition = transform.position;
	}

	public void SetSource (Spawner source) {
		spawner = source.gameObject;
		basePosition = spawner.transform.position;
	}

	void Update () {
		if (!currentlyTracking || currentTarget == null || currentTarget.Equals(null)) {
			findNewTarget();
		}
		if (currentlyTracking) {
			followCurrentTarget(); // Because the items may move in the meantime
		}
	}

	void findNewTarget () {
		currentTarget = FindGameobjects.FindClosest(basePosition, "Resource", Range);
		currentlyTracking = (currentTarget != null);
		if (!currentlyTracking && spawner != null) {
			headHome();
		}
	}

	void followCurrentTarget () {
		if (currentTarget != null) {
			GetComponent<NavMeshAgent>().SetDestination(currentTarget.transform.position);
		}
	}

	void headHome () {
		currentTarget = spawner;
		followCurrentTarget();
	}
}
