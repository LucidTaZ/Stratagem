using UnityEngine;

[System.Serializable]
public class TargetDomain {
	public float Range = Mathf.Infinity;
	public string[] Tags;

	public Vector3 Center;
	public Team FriendlyTeam;

	public void CopyFrom (TargetDomain that) {
		Range = that.Range;
		Tags = that.Tags;
		Center = that.Center;
		FriendlyTeam = that.FriendlyTeam;
	}

	public GameObject findTarget () {
		// Find target within Range from Center, closest to Center
		return findClosestEnemy(Center, Center, Tags, FriendlyTeam, Range);
	}

	public GameObject findTarget (Vector3 closeTo) {
		// Find target within Range from Center, closest to closeTo
		return findClosestEnemy(Center, closeTo, Tags, FriendlyTeam, Range);
	}

	static GameObject findClosestEnemy (Vector3 center, Vector3 closeTo, string[] tags, Team team, float radius = Mathf.Infinity) {
		GameObject result = null;

		float limit = radius*radius;
		float closest = Mathf.Infinity;

		foreach (string tag in tags) {
			GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
			foreach (GameObject go in gos) {
				if (go.activeInHierarchy) {
					BelongsToTeam btt = go.GetComponent<BelongsToTeam>();
					if (btt == null || !team.IsFriendsWith(btt.team)) {
						float distanceSq = (go.transform.position - closeTo).sqrMagnitude;
						float centerDistanceSq = (go.transform.position - center).sqrMagnitude;
						if (distanceSq < closest && centerDistanceSq < limit) {
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
