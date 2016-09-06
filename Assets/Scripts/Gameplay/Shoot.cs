using UnityEngine;
using UnityEngine.Networking;

public class Shoot : NetworkBehaviour {

	public GameObject Projectile;
	public Transform[] EjectionPoints;
	int lastEjectionPoint;

	public float Velocity = 50f;

	public float Cooldown = 0.1f;
	float cooldownTimer = 0f;

	bool isPlayer;

	PlayerState playerState;

	public bool IsOnCooldown () {
		return cooldownTimer >= Time.time;
	}

	void Awake () {
		isPlayer = CompareTag("Player");
		if (isPlayer) {
			playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
			Debug.Assert(playerState != null);
		}

		Debug.Assert(EjectionPoints.Length > 0);
	}

	void Update () {
		if (isPlayer && hasAuthority && Input.GetButton("Shoot") && playerState.CanShoot) {
			Vector3 direction = Camera.main.transform.rotation * new Vector3(0f, 0f, 2f);
			TryToShootAtDirection(direction);
		}
	}

	public void TryToShootAtDirection (Vector3 direction) {
		if (cooldownTimer < Time.time) {
			CmdShootAtDirection(direction, ClientScene.readyConnection.connectionId);
			shootAtDirection(direction);
			cooldownTimer = Time.time + Cooldown;
		}
	}

	[Command(channel=2)] // AllCostDelivery
	public void CmdShootAtDirection (Vector3 direction, int connectionId) {
		RpcAnyPlayerShoots(direction, connectionId);
	}

	[ClientRpc(channel=2)] // AllCostDelivery
	public void RpcAnyPlayerShoots (Vector3 direction, int connectionId) {
		if (connectionId == ClientScene.readyConnection.connectionId) {
			// Ignore own message, since we already performed the local shot when sending the message
			return;
		}
		shootAtDirection(direction);
	}

	void shootAtDirection (Vector3 direction) {
		sendProjectile(direction);
		setNextEjectionPoint();
	}

	void sendProjectile (Vector3 direction) {
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
