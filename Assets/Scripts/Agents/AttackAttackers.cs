using UnityEngine;

// Attack anything that can deal damage to our team
public class AttackAttackers : CombatUnit {
	public float Range = Mathf.Infinity;

	public string[] Tags = {"Attacker", "Player"};

	protected override GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, Tags, Range);
		}
		return FindGameobjects.FindClosestEnemy(basePosition, Tags, btt.team, Range);
	}
}
