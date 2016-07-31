using UnityEngine;

[System.Serializable]
public class StructureItem : Item {
	public GameObject Structure;

	override public void Use (GameObject user) {
		base.Use(user);
		PlaceStructure ps = user.AddComponent<PlaceStructure>();
		ps.Subject = Structure;
	}
}
