using UnityEngine;
using UnityEngine.Networking;

public class CreateLobbyPlayer : NetworkBehaviour {
	public GameObject PlayerPrefab;

	GameObject playerInstance;

	void Start () {
		if (!NetworkServer.active) {
			return;
		}
		playerInstance = Instantiate(PlayerPrefab);
		playerInstance.transform.position = transform.position;
		playerInstance.transform.rotation = transform.rotation;
		NetworkServer.SpawnWithClientAuthority(playerInstance, connectionToClient);
	}

	void OnDestroy () {
		if (!NetworkServer.active) {
			return;
		}
		NetworkServer.Destroy(playerInstance);
	}

	public GameObject getInstance () {
		return playerInstance;
	}
}
