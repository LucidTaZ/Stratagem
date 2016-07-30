using UnityEngine;

public class Attack : MonoBehaviour {
	public float ShootingRangeMax = Mathf.Infinity;
	float shootingRangeMaxSq;

	FindTarget findTarget;
	Shoot shoot;

	Vector3 ejectionPoint;

	void Start () {
		shootingRangeMaxSq = ShootingRangeMax * ShootingRangeMax;

		findTarget = GetComponent<FindTarget>();
		Debug.Assert(findTarget != null);

		shoot = GetComponent<Shoot>();
		Debug.Assert(shoot != null);
	}

	void Update () {
		if (findTarget.CurrentTarget != null) {
			bool closeEnough = (findTarget.CurrentTarget.transform.position - transform.position).sqrMagnitude < shootingRangeMaxSq;
			if (closeEnough) {
				Vector3 direction = aim();
				tryToAttack(direction);
			} else {
				findTarget.ForgetCurrentTarget();
			}
		}
	}

	Vector3 aim () {
		Collider targetCollider = findTarget.CurrentTarget.GetComponent<Collider>();

		// Adjust for the fact that the bullet leaves us not from our origin (Should get this from Shoot script?)
		Vector3 ejectionPoint = shoot.EjectionPoint;
		Vector3 ejectionPointWorld = transform.TransformPoint(ejectionPoint);

		// Find the point we want to shoot
		Vector3 closestPoint = targetCollider.ClosestPointOnBounds(ejectionPointWorld);
		Vector3 positionDifference = closestPoint - ejectionPointWorld;

		// Adjust for gravity of the bullet
		Vector3 gravityAdjustment = (Vector3.up * 1.875f / shoot.Velocity) * positionDifference.magnitude; // Empirically chosen

		return (positionDifference + gravityAdjustment).normalized;
	}

	void tryToAttack (Vector3 direction) {
		shoot.TryToShootAtDirection(direction);
	}
}
