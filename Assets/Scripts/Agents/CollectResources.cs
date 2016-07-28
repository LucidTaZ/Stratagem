﻿using UnityEngine;

public class CollectResources : Unit {
	public float Range = Mathf.Infinity;

	protected override GameObject findNewTarget () {
		return FindGameobjects.FindClosest(basePosition, "Resource", Range);
	}
}