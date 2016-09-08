using UnityEngine;
using UnityEngine.Networking;

public class BelongsToTeam : NetworkBehaviour {
	[SyncVar]
	public Team team;

	public void CopyFrom (BelongsToTeam that) {
		team = that.team;
	}

	void Start () {
		changeMaterialColor();
	}

	void changeMaterialColor () {
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>(true)) {
			if (renderer.GetComponent<TextMesh>() == null) { // Prevent coloring the text, somehow making it invisible
				renderer.material.color = team.color;
			}
		}
	}

	public void UpdateTeamColor (Color color) {
		team.color = color;
		changeMaterialColor();
	}
}
