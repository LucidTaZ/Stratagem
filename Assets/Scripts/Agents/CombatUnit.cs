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
		return ((currentTarget.transform.position - transform.position) + (Vector3.up * 0.5f)).normalized;
	}

	void tryToAttack (Vector3 direction) {
		Shoot shoot = GetComponent<Shoot>();
		if (shoot != null) {
			shoot.TryToShootAtDirection(direction);
		}
	}
}
