using UnityEngine;

public class ItemFactory : MonoBehaviour {
	public GameObject WorkerSpawner;
	public GameObject DefenderSpawner;
	public GameObject OffenderSpawner;
	public GameObject SiegerSpawner;
	public GameObject DefenseTower;

	public GameObject LootTemplate;

	public Sprite ZenyIcon;
	public Sprite WorkerSpawnerIcon;
	public Sprite DefenderSpawnerIcon;
	public Sprite OffenderSpawnerIcon;
	public Sprite SiegerSpawnerIcon;
	public Sprite DefenseTowerIcon;

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
		} else if (itemId.Name == "Defense Tower") {
			StructureItem item = new StructureItem();
			item.Name = itemId.Name;
			item.Structure = DefenseTower;
			return item;
		} else {
			Item item = new Item();
			item.Name = "Unknown (" + itemId.Name + ")";
			Debug.LogWarning("Unknown item ID: " + itemId.Name);
			return item;
		}
	}

	public Sprite GetIcon (ItemIdentifier itemId) {
		switch (itemId.Name) {
		case "Zeny": return ZenyIcon;
		case "Worker Spawner": return WorkerSpawnerIcon;
		case "Defender Spawner": return DefenderSpawnerIcon;
		case "Offender Spawner": return OffenderSpawnerIcon;
		case "Sieger Spawner": return SiegerSpawnerIcon;
		case "Defense Tower": return DefenseTowerIcon;
		}
		Debug.LogError("Icon not found for " + itemId.Name);
		return ZenyIcon;
	}

	public GameObject CreateLootable (ItemIdentifier itemId) {
		GameObject result = Instantiate(LootTemplate);
		Lootable lootable = result.GetComponent<Lootable>();
		lootable.ItemToLoot = itemId;
		// Note: We could set a mesh here if we want
		return result;
	}
}
