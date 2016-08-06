using UnityEngine;
using UnityEngine.UI;

public class StructureConsole : MonoBehaviour {
	public Button ButtonPrefab;

	public GameObject DebugStructureLoot;

	Canvas canvas;

	void Start () {
		canvas = GetComponentInChildren<Canvas>();
		Debug.Assert(canvas != null);

		Debug.Assert(DebugStructureLoot.GetComponent<Lootable>() != null);

		buildContents();
	}

	void buildContents () {
		Button button = Instantiate(ButtonPrefab);
		button.transform.SetParent(canvas.transform, false);

		button.onClick.AddListener(OnPurchaseButtonClicked);

		Text label = button.GetComponentInChildren<Text>();
		label.text = "Get debug item";
	}

	void OnPurchaseButtonClicked () {
		Item item = ItemFactory.Instance().Create(DebugStructureLoot.GetComponent<Lootable>().ItemToLoot);
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Inventory inventory = player.GetComponent<Inventory>();
		inventory.Add(item);
		Debug.Log("Item given");
	}
}
