using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TeamConsole : MonoBehaviour {
	public Text TeamIdText;
	string teamIdFormat;
	int teamId = 1;

	void Start () {
		Debug.Assert(TeamIdText != null);
		teamIdFormat = TeamIdText.text;
		updateTeamIdDisplay();
	}

	GameObject getLocalLobbyPlayer () {
		if (ClientScene.localPlayers.Count == 0) {
			return null;
		}
		return ClientScene.localPlayers[0].gameObject;
	}

	public void TeamIdChanged (Slider slider) {
		teamId = Mathf.RoundToInt(slider.value);
		updateTeamIdDisplay();
		sendTeamChoiceToServer();
	}

	void sendTeamChoiceToServer () {
		GameObject player = getLocalLobbyPlayer();
		Debug.Assert(player != null);
		LobbyTeam lt = player.GetComponent<LobbyTeam>();
		Debug.Assert(lt != null);

		lt.CmdSetTeamId(teamId);
		lt.CmdSetTeamColor(getDefaultTeamColor());
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

	Color getDefaultTeamColor () {
		switch (teamId) {
			case 0: return new Color(62f / 255, 223f / 255, 98f / 255); // Green
			case 1: return new Color(97f / 255, 137f / 255, 255f / 255); // Blue
			case 2: return new Color(255f / 255, 99f / 255, 148f / 255); // Red
			case 3: return new Color(83f / 255, 0f / 255, 255f / 255); // Purple
		}
		return new Color();
	}
}
