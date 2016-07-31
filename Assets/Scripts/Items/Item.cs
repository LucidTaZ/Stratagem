using UnityEngine;

[System.Serializable]
public class Item {
	public string Name;

	public GameObject GroundItem;

	public GameObject InstantiateGroundItem () {
		return GameObject.Instantiate(GroundItem);
	}

	virtual public void Use (GameObject user) {
		Debug.Log("Using item " + Name + " by user " + user.name);
	}
}
