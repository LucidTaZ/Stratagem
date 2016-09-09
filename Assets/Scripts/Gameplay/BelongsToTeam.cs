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
				setColorKeepingAlpha(renderer.material, team.color);

			}
		}
	}

	void setColorKeepingAlpha (Material material, Color color) {
		if (!material.HasProperty("_Color")) {
			return;
		}
		material.color = new Color(
			color.r,
			color.g,
			color.b,
			material.color.a
		);
	}

	public void UpdateTeamColor (Color color) {
		team.color = color;
		changeMaterialColor();
	}
}
