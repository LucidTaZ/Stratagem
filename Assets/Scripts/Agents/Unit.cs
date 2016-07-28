using UnityEngine;

abstract public class Unit : MonoBehaviour, Spawnable {
	public GameObject currentTarget;

	protected Vector3 basePosition;
	Spawner spawner;

	protected bool currentlyTracking; // When false, found no resource, so head home

	protected NavMeshAgent nav;

	protected abstract GameObject findNewTarget();

	public void SetSource (Spawner source) {
		spawner = source;
		basePosition = spawner.gameObject.transform.position;
	}

	protected virtual void Start () {
		basePosition = transform.position;
		nav = GetComponent<NavMeshAgent>();
	}

	protected virtual void Update () {
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
		if (currentTarget != null && nav != null) {
			nav.SetDestination(currentTarget.transform.position);
		}
	}

	void headHome () {
		currentTarget = spawner.gameObject;
		followCurrentTarget();
	}
}
