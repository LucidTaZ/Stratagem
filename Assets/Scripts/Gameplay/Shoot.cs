using UnityEngine;
using UnityEngine.Networking;

public class Shoot : NetworkBehaviour {

	public GameObject Projectile;
	public Transform[] EjectionPoints;
	int lastEjectionPoint;

	public float Velocity = 50f;

	public float Cooldown = 0.1f;
	float cooldownTimer = 0f;

	public float MaxDeviationAngle = 180.0f; // Maximum angle that may be deviated from the "forward" direction. If this is set to a low number, the entity can only shoot forward, not sideways.

	bool isPlayer;

	PlayerState playerState;

	public bool IsOnCooldown () {
		return cooldownTimer >= Time.time;
	}

	// Is the weapon capable of shooting in the given direction, based on the maximum allowed angle?
	public bool IsSufficientlyAligned (Vector3 direction) {
		Transform ejectionPoint = GetCurrentEjectionPoint();
		Vector3 worldGunForward = ejectionPoint.TransformDirection(Vector3.forward);
		float angle = Vector3.Angle(worldGunForward, direction);
		return angle <= MaxDeviationAngle;
	}

	void Awake () {
		isPlayer = CompareTag("Player");
		if (isPlayer) {
			playerState = PlayerState.Instance();
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
		if (!IsOnCooldown() && IsSufficientlyAligned(direction)) {
			CmdShootAtDirection(direction, NetworkManager.singleton.client.connection.connectionId);
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
		Transform ejectionPoint = GetCurrentEjectionPoint();
		newProjectile.transform.position = ejectionPoint.position;

		newProjectile.GetComponent<Rigidbody>().AddForce(direction.normalized * Velocity, ForceMode.VelocityChange);

		DealsDamage dealsDamage = newProjectile.GetComponent<DealsDamage>();
		if (dealsDamage != null) {
			dealsDamage.SetShooter(gameObject);
		} else {
			Debug.LogWarning("Firing a projectile that deals no damage");
		}
	}

	public Transform GetCurrentEjectionPoint () {
		return EjectionPoints[lastEjectionPoint];
	}

	void setNextEjectionPoint () {
		lastEjectionPoint = (++lastEjectionPoint) % EjectionPoints.Length;
	}
}
