using UnityEngine;

public class ItemFactory : MonoBehaviour {
	public GameObject WorkerSpawner;
	public GameObject DefenderSpawner;
	public GameObject OffenderSpawner;
	public GameObject SiegerSpawner;

	public static ItemFactory Instance () {
		GameObject itemFactoryHolder = GameObject.FindGameObjectWithTag("ItemFactory");
		Debug.Assert(itemFactoryHolder != null);
		ItemFactory itemFactory = itemFactoryHolder.GetComponent<ItemFactory>();
		Debug.Assert(itemFactory != null);
		return itemFactory;
	}

	public Item Create (ItemIdentifier itemId) {
		if (itemId.Name == "Zeny") {
			Item item = new Item();
			item.Name = itemId.Name;
			return item;
		} else if (itemId.Name == "Worker Spawner") {
			StructureItem item = new StructureItem();
			item.Name = itemId.Name;
			item.Structure = WorkerSpawner;
			return item;
		} else if (itemId.Name == "Defender Spawner") {
			StructureItem item = new StructureItem();
			item.Name = itemId.Name;
			item.Structure = DefenderSpawner;
			return item;
		} else if (itemId.Name == "Offender Spawner") {
			StructureItem item = new StructureItem();
			item.Name = itemId.Name;
			item.Structure = OffenderSpawner;
			return item;
		} else if (itemId.Name == "Sieger Spawner") {
			StructureItem item = new StructureItem();
			item.Name = itemId.Name;
			item.Structure = SiegerSpawner;
			return item;
		} else {
			Item item = new Item();
			item.Name = "Unknown (" + itemId.Name + ")";
			Debug.LogWarning("Unknown item ID: " + itemId.Name);
			return item;
		}
	}
}
