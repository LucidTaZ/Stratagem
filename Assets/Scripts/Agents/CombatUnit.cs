using UnityEngine;

abstract public class CombatUnit : Unit {
	public float ShootingRangeMax = 10;
	float shootingRangeMaxSq;

	public float ApproachDistanceMin = 5;
	float approachDistanceMinSq;

	abstract protected override GameObject findNewTarget ();

	protected override void Start () {
		base.Start();
		shootingRangeMaxSq = ShootingRangeMax * ShootingRangeMax;
		approachDistanceMinSq = ApproachDistanceMin * ApproachDistanceMin;
	}

	protected override void Update () {
		base.Update();
		approach();
		attack();
	}

	void approach () {
		if (currentTarget == null || !currentlyTracking) {
			nav.Resume();
			return;
		}

		bool closeEnough = (currentTarget.transform.position - transform.position).sqrMagnitude < approachDistanceMinSq;
		if (closeEnough) {
			nav.Stop();
		} else {
			nav.Resume();
		}
	}

	void attack () {
		if (currentTarget != null && currentlyTracking) {
			bool closeEnough = (currentTarget.transform.position - transform.position).sqrMagnitude < shootingRangeMaxSq;
			if (closeEnough) {
				Vector3 direction = aim();
				tryToAttack(direction);
			}
		}
	}

	Vector3 aim () {
		Collider targetCollider = currentTarget.GetComponent<Collider>();

		// Adjust for the fact that the bullet leaves us not from our origin (Should get this from Shoot script?)
		Vector3 nozzleLocalPosition = new Vector3(0f, 1f, 0f);
		Vector3 nozzleWorldPosition = transform.TransformPoint(nozzleLocalPosition);

		// Find the point we want to shoot
		Vector3 closestPoint = targetCollider.ClosestPointOnBounds(nozzleWorldPosition);
		Vector3 positionDifference = closestPoint - nozzleWorldPosition;

		// Adjust for gravity of the bullet
		Vector3 gravityAdjustment = (Vector3.up * 0.075f) * positionDifference.magnitude; // Empirically chosen

		return (positionDifference + gravityAdjustment).normalized;
	}

	void tryToAttack (Vector3 direction) {
		Shoot shoot = GetComponent<Shoot>();
		if (shoot != null) {
			shoot.TryToShootAtDirection(direction);
		}
	}
}
