using UnityEngine;
using UnityEngine.Networking;

public class LobbyTeam : NetworkBehaviour {
	public Team team;

	[Command]
	public void CmdSetTeamId (int id) {
		team.id = id;
	}
}
