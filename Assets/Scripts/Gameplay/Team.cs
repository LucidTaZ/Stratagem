using UnityEngine;
using System;

[Serializable]
public struct Team {
	public int id;
	public string name;
	public Color color;

	public bool IsFriendsWith (Team that) {
		return id == that.id;
	}
}
