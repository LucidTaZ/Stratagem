using UnityEngine;

public class AttackUnits : Unit {
	public float Range = Mathf.Infinity;

	protected override GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, "Enemy", Range);
		}
		return FindGameobjects.FindClosestEnemy(basePosition, "Enemy", btt.team, Range);
	}
}
