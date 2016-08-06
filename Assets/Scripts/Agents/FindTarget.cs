﻿using UnityEngine;

public class FindTarget : MonoBehaviour {
	public float Range = Mathf.Infinity;

	public string[] Tags;

	[HideInInspector]
	public GameObject CurrentTarget;

	Vector3 basePosition;

	public void ForgetCurrentTarget () {
		CurrentTarget = null;
	}

	void Start () {
		basePosition = transform.position;
	}

	void Update () {
		if (CurrentTarget == null || CurrentTarget.Equals(null)) {
			CurrentTarget = findNewTarget();
		}
	}

	GameObject findNewTarget () {
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt == null) {
			return FindGameobjects.FindClosest(basePosition, Tags, Range);
		}
		return FindGameobjects.FindClosestEnemy(basePosition, Tags, btt.team, Range);
	}
}