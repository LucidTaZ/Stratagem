using UnityEngine;

public class FindGameobjects {
	public static GameObject FindClosest (Vector3 center, string tag, float radius = Mathf.Infinity) {
		GameObject result = null;
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);

		float limit = radius*radius;
		float closest = Mathf.Infinity;
		foreach (GameObject go in gos) {
			float distanceSq = (go.transform.position - center).sqrMagnitude;
			if (distanceSq < closest && distanceSq < limit) {
				closest = distanceSq;
				result = go;
			}
		}
		return result;
	}

	public static GameObject FindClosestEnemy (Vector3 center, string tag, Team team, float radius = Mathf.Infinity) {
		GameObject result = null;
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);

		float limit = radius*radius;
		float closest = Mathf.Infinity;
		foreach (GameObject go in gos) {
			BelongsToTeam btt = go.GetComponent<BelongsToTeam>();
			if (btt == null || !team.IsFriendsWith(btt.team)) {
				float distanceSq = (go.transform.position - center).sqrMagnitude;
				if (distanceSq < closest && distanceSq < limit) {
					closest = distanceSq;
					result = go;
				}
			}
		}
		return result;
	}
}
