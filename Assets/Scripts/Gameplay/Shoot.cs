using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject Projectile;
	public Transform[] EjectionPoints;
	int lastEjectionPoint;

	public float Velocity = 50f;

	public float Cooldown = 0.1f;
	float cooldownTimer = 0f;

	bool isPlayer;

	PlayerState playerState;

	void Start () {
		isPlayer = CompareTag("Player");
		if (isPlayer) {
			playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
			Debug.Assert(playerState != null);
		}

		Debug.Assert(EjectionPoints.Length > 0);
	}

	void Update () {
		if (isPlayer && Input.GetButton("Shoot") && playerState.CanShoot) {
			TryToShoot();
		}
	}

	public void TryToShoot ()
	{
		if (cooldownTimer < Time.time) {
			doShoot();
			cooldownTimer = Time.time + Cooldown;
			setNextEjectionPoint();
		}
	}

	public void TryToShootAtDirection (Vector3 direction)
	{
		if (cooldownTimer < Time.time) {
			doShootAtDirection(direction);
			cooldownTimer = Time.time + Cooldown;
			setNextEjectionPoint();
		}
	}

	void doShoot ()
	{
		Vector3 direction = Camera.main.transform.rotation * new Vector3(0f, 0f, 2f);
		doShootAtDirection(direction);
	}

	void doShootAtDirection (Vector3 direction) {
		GameObject newProjectile = Instantiate(Projectile);
		Vector3 ejectionPoint = GetCurrentEjectionPoint();
		newProjectile.transform.position = ejectionPoint;
		newProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * Velocity, ForceMode.VelocityChange);

		DealsDamage dealsDamage = newProjectile.GetComponent<DealsDamage>();
		if (dealsDamage != null) {
			dealsDamage.SetShooter(gameObject);
		} else {
			Debug.LogWarning("Firing a projectile that deals no damage");
		}
	}

	public Vector3 GetCurrentEjectionPoint () {
		return EjectionPoints[lastEjectionPoint].position;
	}

	void setNextEjectionPoint () {
		lastEjectionPoint = (++lastEjectionPoint) % EjectionPoints.Length;
	}
}
