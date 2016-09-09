using UnityEngine;
using UnityEngine.Networking;

public class FindGameobjects {
	public static GameObject FindLocalPlayerObject () {
		foreach (GameObject candidate in GameObject.FindGameObjectsWithTag("Player")) {
			if (candidate.GetComponent<NetworkIdentity>().hasAuthority) {
				return candidate;
			}
		}
		return null;
	}

	public static Inventory FindTeamInventory (Team team) {
		GameObject heart = FindTeamHeart(team);
		if (heart == null) {
			Debug.LogWarning("Cannot find team inventory because there is no heart");
			return null;
		}
		Inventory inventory = heart.GetComponent<Inventory>();
		Debug.Assert(inventory != null);
		return inventory;
	}

	public static GameObject FindTeamHeart (Team team) {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Heart")) {
			BelongsToTeam btt = go.GetComponent<BelongsToTeam>();
			Debug.Assert(btt != null);
			if (btt.team.IsSame(team)) {
				return go;
			}
		}
		Debug.LogWarning("Team has no heart? Team = " + team.name + "(" + team.id + ")");
		return null;
	}
}
