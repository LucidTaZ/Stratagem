using UnityEngine;

[System.Serializable]
public class Item {
	public class Stack {
		public Item ContainedItem;
		public int Count;
	}

	public string Name;

	public GameObject GroundItem;

	public GameObject InstantiateGroundItem () {
		return GameObject.Instantiate(GroundItem);
	}

	virtual public bool CanUse (GameObject user) {
		return false;
	}

	virtual public void Use (GameObject user) {
		Debug.Log("Using item " + Name + " by user " + user.name);
	}
}
