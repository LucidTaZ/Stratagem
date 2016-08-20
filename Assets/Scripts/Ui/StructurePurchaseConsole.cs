using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class StructurePurchaseConsole : NetworkBehaviour {
	public Button ButtonPrefab;

	public Canvas canvas;

	public PurchaseHandler Handler;

	void Start () {
		Debug.Assert(ButtonPrefab != null);
		Debug.Assert(canvas != null);
		Debug.Assert(Handler != null);

		buildContents();
	}

	void buildContents () {
		int i = 0;
		foreach (PurchaseableItem purchaseableItem in Handler.Assortment) {
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

		button.onClick.AddListener(() => purchaseButtonClicked(purchaseableItem));

		Text label = button.GetComponentInChildren<Text>();
		label.text = caption;
	}

	void purchaseButtonClicked (PurchaseableItem purchaseableItem) {
		GameObject player = FindGameobjects.FindLocalPlayerObject();
		Debug.Assert(player != null);
		PurchaseCustomer pc = player.GetComponent<PurchaseCustomer>();
		Debug.Assert(pc != null);
		pc.Purchase(purchaseableItem, Handler);
	}
}
