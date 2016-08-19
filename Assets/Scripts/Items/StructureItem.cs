using UnityEngine;

[System.Serializable]
public class StructureItem : Item {
	public GameObject Structure;

	override public bool CanUse (GameObject user) {
		return !user.GetComponent<PlaceStructure>().enabled; // False when already placing
	}

	override public void Use (GameObject user) {
		base.Use(user);
		PlaceStructure ps = user.GetComponent<PlaceStructure>();
		Debug.Assert(ps != null);
		ps.Subject = Structure;
		ps.enabled = true;
	}
}
