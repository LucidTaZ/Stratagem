using UnityEngine;

public class AttackPlayer : Unit {
	protected override GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, "Player");
		}
		return FindGameobjects.FindClosestEnemy(basePosition, "Player", btt.team);
	}
}
