using UnityEngine;

public class BelongsToTeam : MonoBehaviour {
	public Team team;

	public void CopyFrom (BelongsToTeam that) {
		team = that.team;
	}

	void Start () {
		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
			renderer.material.color = team.color;
		}
	}
}
