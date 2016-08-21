using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TeamConsole : MonoBehaviour {
	public Text TeamIdText;
	string teamIdFormat;
	int teamId;

	void Start () {
		Debug.Assert(TeamIdText != null);
		teamIdFormat = TeamIdText.text;
		updateTeamIdDisplay();
	}

	GameObject getLocalLobbyPlayer () {
		Debug.Log(ClientScene.localPlayers.Count);
		return ClientScene.localPlayers[0].gameObject;
	}

	public void TeamIdChanged (Slider slider) {
		teamId = Mathf.RoundToInt(slider.value);

		updateTeamIdDisplay();

		GameObject player = getLocalLobbyPlayer();
		Debug.Assert(player != null);
		LobbyTeam lt = player.GetComponent<LobbyTeam>();
		Debug.Assert(lt != null);

		lt.CmdSetTeamId(teamId);
	}

	public void ReadyChanged (Toggle toggle) {
		bool isReady = toggle.isOn;

		GameObject player = getLocalLobbyPlayer();
		Debug.Assert(player != null);
		NetworkLobbyPlayer nlp = player.GetComponent<NetworkLobbyPlayer>();
		Debug.Assert(nlp != null);

		if (isReady) {
			nlp.SendReadyToBeginMessage();
		} else {
			nlp.SendNotReadyToBeginMessage();
		}
	}

	void updateTeamIdDisplay () {
		TeamIdText.text = string.Format(teamIdFormat, teamId);
	}
}
