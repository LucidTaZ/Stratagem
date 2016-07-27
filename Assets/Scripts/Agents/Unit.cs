using UnityEngine;

abstract public class Unit : MonoBehaviour, Spawnable {
	public GameObject currentTarget;

	protected Vector3 basePosition;
	Spawner spawner;

	protected bool currentlyTracking; // When false, found no resource, so head home

	protected abstract GameObject findNewTarget();

	public void SetSource (Spawner source) {
		spawner = source;
		basePosition = spawner.gameObject.transform.position;
	}

	void Start () {
		basePosition = transform.position;
	}

	void Update () {
		if (!currentlyTracking || currentTarget == null || currentTarget.Equals(null)) {
			tryToFindNewTarget();
		}
		if (currentlyTracking) {
			followCurrentTarget(); // Because the items may move in the meantime
		}
	}

	void tryToFindNewTarget () {
		currentTarget = findNewTarget();
		currentlyTracking = (currentTarget != null);
		if (!currentlyTracking && spawner != null) {
			headHome();
		}
	}

	void OnDestroy () {
		if (spawner != null) {
			spawner.OnSubjectDestroyed(gameObject);
		}
	}

	void followCurrentTarget () {
		if (currentTarget != null) {
			GetComponent<NavMeshAgent>().SetDestination(currentTarget.transform.position);
		}
	}

	void headHome () {
		currentTarget = spawner.gameObject;
		followCurrentTarget();
	}
}
