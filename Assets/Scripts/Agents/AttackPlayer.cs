using UnityEngine;

public class AttackPlayer : CombatUnit {
	public float Range = Mathf.Infinity;

	protected override GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, "Player", Range);
		}
		return FindGameobjects.FindClosestEnemy(basePosition, "Player", btt.team, Range);
	}
}
