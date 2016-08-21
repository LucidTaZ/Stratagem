using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager {
	public override bool OnLobbyServerSceneLoadedForPlayer (GameObject lobbyPlayer, GameObject gamePlayer) {
		LobbyTeam lt = lobbyPlayer.GetComponent<LobbyTeam>();
		Debug.Assert(lt != null);
		Team team = lt.team;

		CreateOrJoinTeam cjt = gamePlayer.GetComponent<CreateOrJoinTeam>();
		Debug.Assert(cjt != null);
		cjt.team = team;

		return true;
	}
}
