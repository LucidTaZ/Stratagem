using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerSpawner : NetworkBehaviour {

	class SpawnState {
		public bool isAlive = false;
		public float spawnTime = Time.time;
		public GameObject instance = null;
	}

	public GameObject PlayerPrefab;
	public Transform SpawnLocation;

	public float CooldownSeconds = 10;

	Dictionary<NetworkConnection, SpawnState> spawnStates = new Dictionary<NetworkConnection, SpawnState>();

	public void RegisterPlayer (NetworkConnection connection) {
		spawnStates[connection] = new SpawnState();
	}

	public void UnregisterPlayer (NetworkConnection connection) {
		spawnStates.Remove(connection);
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}
		checkStatesAndSpawnIfNeeded();
	}

	void checkStatesAndSpawnIfNeeded () {
		// Store the keys beforehand, so we can modify the dictionary during iteration
		List<NetworkConnection> connections = new List<NetworkConnection>(spawnStates.Keys);
		foreach (NetworkConnection connection in connections) {
			SpawnState spawnState = spawnStates[connection];
			if (!spawnState.isAlive && spawnState.spawnTime <= Time.time) {
				PerformSpawn(connection);
			}
		}
	}

	void PerformSpawn (NetworkConnection connection) {
		GameObject subject = Instantiate(PlayerPrefab);
		subject.transform.position = SpawnLocation.position;
		subject.transform.rotation = SpawnLocation.rotation;

		Spawnable spawnable = subject.GetComponent<Spawnable>();
		if (spawnable != null) {
			spawnable.SetHeart(this);
		}
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		BelongsToTeam childBtt = subject.GetComponent<BelongsToTeam>();
		if (btt != null && childBtt != null) {
			childBtt.CopyFrom(btt);
		}

		NetworkServer.SpawnWithClientAuthority(subject, connection);

		spawnStates[connection].isAlive = true;
		spawnStates[connection].instance = subject;
	}

	public void OnSubjectDestroyed (GameObject subject) {
		NetworkConnection connection = findSpawnStateKeyFor(subject);
		spawnStates[connection].isAlive = false;
		spawnStates[connection].spawnTime = Time.time + CooldownSeconds;
	}

	NetworkConnection findSpawnStateKeyFor (GameObject subject) {
		foreach (KeyValuePair<NetworkConnection, SpawnState> connectionAndState in spawnStates) {
			if (connectionAndState.Value.instance == subject) {
				return connectionAndState.Key;
			}
		}
		Debug.LogError("Did not find spawnState for " + subject);
		return null;
	}
}
