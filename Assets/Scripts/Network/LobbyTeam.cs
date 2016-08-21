using UnityEngine;
using UnityEngine.Networking;

public class LobbyTeam : NetworkBehaviour {
	public Team team;

	[Command]
	public void CmdSetTeamId (int id) {
		team.id = id;
	}

	[Command]
	public void CmdSetTeamColor (Color color) {
		team.color = color;
		RpcSetTeamColor(GetComponent<CreateLobbyPlayer>().getInstance().GetComponent<NetworkIdentity>().netId, color);
	}

	[ClientRpc]
	public void RpcSetTeamColor (NetworkInstanceId netId, Color color) {
		ClientScene.FindLocalObject(netId).GetComponent<BelongsToTeam>().UpdateTeamColor(color);
	}
}
