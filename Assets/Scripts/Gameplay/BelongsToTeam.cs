using UnityEngine;

public class BelongsToTeam : MonoBehaviour {
	public Team team;

	public void CopyFrom (BelongsToTeam that) {
		team = that.team;
	}

	void Start () {
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
			if (renderer.gameObject.GetComponent<TextMesh>() == null) { // Prevent coloring the text, somehow making it invisible
				renderer.material.color = team.color;
			}
		}
	}
}
