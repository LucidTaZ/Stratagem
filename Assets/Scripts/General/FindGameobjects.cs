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

	public static GameObject FindClosest (Vector3 center, string tag, float radius = Mathf.Infinity) {
		return FindClosest(center, new string[]{tag}, radius);
	}

	public static GameObject FindClosest (Vector3 center, string[] tags, float radius = Mathf.Infinity) {
		GameObject result = null;

		float limit = radius*radius;
		float closest = Mathf.Infinity;

		foreach (string tag in tags) {
			GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
			foreach (GameObject go in gos) {
				if (go.activeInHierarchy) {
					float distanceSq = (go.transform.position - center).sqrMagnitude;
					if (distanceSq < closest && distanceSq < limit) {
						closest = distanceSq;
						result = go;
					}
				}
			}
		}

		return result;
	}

	public static GameObject FindClosestEnemy (Vector3 center, string tag, Team team, float radius = Mathf.Infinity) {
		return FindClosestEnemy(center, new string[]{tag}, team, radius);
	}

	public static GameObject FindClosestEnemy (Vector3 center, string[] tags, Team team, float radius = Mathf.Infinity) {
		GameObject result = null;

		float limit = radius*radius;
		float closest = Mathf.Infinity;

		foreach (string tag in tags) {
			GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
			foreach (GameObject go in gos) {
				if (go.activeInHierarchy) {
					BelongsToTeam btt = go.GetComponent<BelongsToTeam>();
					if (btt == null || !team.IsFriendsWith(btt.team)) {
						float distanceSq = (go.transform.position - center).sqrMagnitude;
						if (distanceSq < closest && distanceSq < limit) {
							closest = distanceSq;
							result = go;
						}
					}
				}
			}
		}

		return result;
	}
}
