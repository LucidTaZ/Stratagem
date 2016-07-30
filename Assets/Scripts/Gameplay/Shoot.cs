using UnityEngine;

public class Shoot : MonoBehaviour {

	public GameObject Projectile;

	public float Velocity = 50f;

	public float Cooldown = 0.1f;
	float cooldownTimer = 0f;

	bool isPlayer;

	void Start () {
		isPlayer = CompareTag("Player");
	}

	void Update () {
		if (isPlayer && Input.GetButton("Shoot")) {
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
		Vector3 relativeSpawnOrigin = new Vector3(0f, 1f, 0f);
		newProjectile.transform.position = transform.position + direction.normalized + transform.rotation * relativeSpawnOrigin;
		newProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * Velocity, ForceMode.VelocityChange);
	}
}
