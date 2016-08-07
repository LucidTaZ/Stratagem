using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject Projectile;
	public Vector3 EjectionPoint;

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
		}
	}

	public void TryToShootAtDirection (Vector3 direction)
	{
		if (cooldownTimer < Time.time) {
			doShootAtDirection(direction);
			cooldownTimer = Time.time + Cooldown;
		}
	}

	void doShoot ()
	{
		Vector3 direction = Camera.main.transform.rotation * new Vector3(0f, 0f, 2f);
		doShootAtDirection(direction);
	}

	void doShootAtDirection (Vector3 direction) {
		GameObject newProjectile = Instantiate(Projectile);
		newProjectile.transform.position = transform.position + direction.normalized + transform.rotation * EjectionPoint;
		newProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * Velocity, ForceMode.VelocityChange);

		DealsDamage dealsDamage = newProjectile.GetComponent<DealsDamage>();
		if (dealsDamage != null) {
			dealsDamage.SetShooter(gameObject);
		} else {
			Debug.LogWarning("Firing a projectile that deals no damage");
		}
	}
}
