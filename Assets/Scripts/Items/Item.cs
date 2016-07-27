using UnityEngine;

[System.Serializable]
public class Item {
	public string Name;

	public GameObject GroundItem;

	public GameObject InstantiateGroundItem () {
		return GameObject.Instantiate(GroundItem);
	}
}
