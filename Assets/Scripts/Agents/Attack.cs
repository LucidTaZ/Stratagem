using UnityEngine;
using UnityEngine.Networking;

public class Attack : NetworkBehaviour {
	public float ShootingRangeMax = Mathf.Infinity;
	float shootingRangeMaxSq;

	FindTarget findTarget;
	Shoot shoot;

	void Start () {
		shootingRangeMaxSq = ShootingRangeMax * ShootingRangeMax;

		findTarget = GetComponent<FindTarget>();
		Debug.Assert(findTarget != null);

		shoot = GetComponent<Shoot>();
		Debug.Assert(shoot != null);
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}
		if (findTarget.HasTarget()) {
			bool closeEnough = (findTarget.CurrentTarget.transform.position - transform.position).sqrMagnitude < shootingRangeMaxSq;
			if (closeEnough) {
				if (!shoot.IsOnCooldown()) {
					Vector3 direction = aim();
					tryToAttack(direction);
				}
				findTarget.VoteKeepTarget();
			} else {
				// We don't mind if the target is dropped now in favor of another one
				findTarget.VoteForgetTarget();
			}
		}
	}

	Vector3 aim () {
		// Find any collider (target can have multiple) to shoot at. We may refactor this to intelligently select one.
		Collider targetCollider = selectTargetCollider(findTarget.CurrentTarget);

		// Adjust for the fact that the bullet leaves us not from our origin
		Transform ejectionPoint = shoot.GetCurrentEjectionPoint();

		// Find the point we want to shoot
		Vector3 closestPoint = targetCollider.ClosestPointOnBounds(ejectionPoint.position);
		Vector3 positionDifference = closestPoint - ejectionPoint.position;

		// Adjust for gravity of the bullet
		Vector3 gravityAdjustment = (Vector3.up * 1.875f / shoot.Velocity) * positionDifference.magnitude; // Empirically chosen

		return (positionDifference + gravityAdjustment).normalized;
	}

	Collider selectTargetCollider (GameObject target) {
		Hitpoints hitpoints = target.GetComponent<Hitpoints>();
		Debug.Assert(hitpoints != null);
		if (hitpoints.AllowCollisionsInChildren) {
			return target.GetComponentInChildren<Collider>();
		}
		return target.GetComponent<Collider>();
	}

	void tryToAttack (Vector3 direction) {
		shoot.TryToShootAtDirection(direction);
	}
}
