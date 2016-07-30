using UnityEngine;

public class AttackAnything : CombatUnit {
	public float Range = Mathf.Infinity;

	public string[] Tags = {"Attacker", "Player", "Structure", "Worker"};

	protected override GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, Tags, Range);
		}
		return FindGameobjects.FindClosestEnemy(basePosition, Tags, btt.team, Range);
	}
}
