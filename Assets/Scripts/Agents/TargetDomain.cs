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
		return FindGameobjects.FindClosestEnemy(Center, Tags, FriendlyTeam, Range);
	}
}
