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
		ItemIdentifier zeny = new ItemIdentifier("Zeny"); // TODO: Make settable via PurchasableItem

		if (teamInventory.Count(zeny) >= purchaseableItem.Cost) {
			teamInventory.Remove(zeny, purchaseableItem.Cost);
			buyItem(purchaseableItem);
		}
	}

	void buyItem (PurchaseableItem purchaseableItem) {
		PurchaseDropZone dropZone = GetComponentInParent<PurchaseDropZone>();
		if (dropZone == null) {
			Debug.LogError("Please make sure that the structure GameObject has a PurchaseDropZone component.");
		}
		Vector3 dropLocation = dropZone.SampleWorldLocation();
		dropLocation.y = 10f; // TODO: Make dependent on the "build time"

		GameObject loot = ItemFactory.Instance().CreateLootable(purchaseableItem.Identifier);
		loot.transform.position = dropLocation;
	}
}
