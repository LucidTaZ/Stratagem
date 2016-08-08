using UnityEngine;
using UnityEngine.UI;

public class StructurePurchaseConsole : MonoBehaviour {
	public Button ButtonPrefab;

	public Canvas canvas;

	public PurchaseableItem[] Assortment;

	Inventory teamInventory;

	void Start () {
		Debug.Assert(ButtonPrefab != null);
		Debug.Assert(canvas != null);
		Debug.Assert(Assortment != null);

		teamInventory = GameObject.FindGameObjectWithTag("ResourceHolder").GetComponent<Inventory>();
		Debug.Assert(teamInventory != null);

		buildContents();
	}

	void buildContents () {
		int i = 0;
		foreach (PurchaseableItem purchaseableItem in Assortment) {
			string caption = string.Format("{0}: {1}z", purchaseableItem.Identifier.Name, purchaseableItem.Cost);
			buildButton(caption, purchaseableItem, i);
			i++;
		}
	}

	void buildButton (string caption, PurchaseableItem purchaseableItem, int yPos) {
		float buttonHeight = ButtonPrefab.GetComponent<RectTransform>().rect.height;
		float margin = buttonHeight / 10;
		float heightPerButton = (buttonHeight + margin) * ButtonPrefab.GetComponent<RectTransform>().localScale.y;

		Button button = Instantiate(ButtonPrefab);
		button.transform.SetParent(canvas.transform, false);
		button.transform.localPosition = new Vector3(
			button.transform.localPosition.x,
			button.transform.localPosition.y - yPos * heightPerButton,
			button.transform.localPosition.z
		);

		button.onClick.AddListener(() => OnPurchaseButtonClicked(purchaseableItem));

		Text label = button.GetComponentInChildren<Text>();
		label.text = caption;
	}

	void OnPurchaseButtonClicked (PurchaseableItem purchaseableItem) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Debug.Assert(player != null);
		Inventory inventory = player.GetComponent<Inventory>();

		ItemIdentifier zeny = new ItemIdentifier("Zeny"); // TODO: Make settable via PurchasableItem

		if (teamInventory.Count(zeny) >= purchaseableItem.Cost) {
			teamInventory.Remove(zeny, purchaseableItem.Cost);
			Item item = ItemFactory.Instance().Create(purchaseableItem.Identifier);
			inventory.Add(item);
		}
	}
}
