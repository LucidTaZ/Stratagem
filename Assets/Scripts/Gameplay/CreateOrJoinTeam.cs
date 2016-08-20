using UnityEngine;
using UnityEngine.Networking;

public class CreateOrJoinTeam : NetworkBehaviour {
	public Team team;

	public GameObject HeartPrefab;

	void Awake () {
		Debug.Assert(HeartPrefab.CompareTag("Heart"));
	}

	void Start () {
		if (!NetworkServer.active) {
			return;
		}
		GameObject existingHeart = findExistingTeamHeart();
		if (existingHeart != null) {
			registerPlayerAtHeart(existingHeart);
		} else {
			GameObject newHeart = spawnNewHeart();
			registerPlayerAtHeart(newHeart);
		}
	}

	GameObject findExistingTeamHeart () {
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Heart");
		foreach (GameObject go in gos) {
			BelongsToTeam btt = go.GetComponent<BelongsToTeam>();
			if (btt.team.Equals(team)) {
				return go;
			}
		}
		return null;
	}

	void registerPlayerAtHeart (GameObject heart) {
		PlayerSpawner ps = heart.GetComponent<PlayerSpawner>();
		Debug.Assert(ps != null);
		ps.RegisterPlayer(GetComponent<NetworkIdentity>().connectionToClient);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (!NetworkServer.active) {
			return;
		}
		GameObject heart = findExistingTeamHeart();
		PlayerSpawner ps = heart.GetComponent<PlayerSpawner>();
		Debug.Assert(ps != null);
		ps.UnregisterPlayer(GetComponent<NetworkIdentity>().connectionToClient);
	}

	GameObject spawnNewHeart () {
		GameObject instance = Instantiate(HeartPrefab);
		instance.transform.position = transform.position;
		instance.transform.rotation = transform.rotation;

		BelongsToTeam btt = instance.GetComponent<BelongsToTeam>();
		btt.team = team;

		Debug.Assert(btt.enabled);
		NetworkServer.Spawn(instance);

		return instance;
	}
}
