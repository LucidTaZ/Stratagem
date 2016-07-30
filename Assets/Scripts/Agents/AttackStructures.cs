using UnityEngine;

public class AttackStructures : CombatUnit {
	public float Range = Mathf.Infinity;

	protected override GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, "Structure", Range);
		}
		return FindGameobjects.FindClosestEnemy(basePosition, "Structure", btt.team, Range);
	}
}
