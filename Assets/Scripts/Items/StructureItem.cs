using UnityEngine;

[System.Serializable]
public class StructureItem : Item {
	public GameObject Structure;

	override public bool CanUse (GameObject user) {
		return user.GetComponent<PlaceStructure>() == null; // False when already placing
	}

	override public void Use (GameObject user) {
		base.Use(user);
		PlaceStructure ps = user.AddComponent<PlaceStructure>();
		ps.Subject = Structure;
	}
}
