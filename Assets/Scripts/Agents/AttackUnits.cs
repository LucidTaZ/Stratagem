using UnityEngine;

public class AttackUnits : Unit {
	public float Range = Mathf.Infinity;

	protected override GameObject findNewTarget () {
		return FindGameobjects.FindClosest(basePosition, "Enemy", Range);
	}
}
