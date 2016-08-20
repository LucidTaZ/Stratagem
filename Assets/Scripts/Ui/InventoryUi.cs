using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InventoryUi : NetworkBehaviour {
	public GameObject UiContainerPrefab;
	public Button ButtonPrefab;

	Inventory inventory;

	GameObject uiContainer;

	PlayerState playerState;

	void Start () {
		if (!hasAuthority) {
			return;
		}
		inventory = GetComponent<Inventory>();
		Debug.Assert(inventory != null);
		Debug.Assert(UiContainerPrefab != null);
		Debug.Assert(ButtonPrefab != null);

		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
		Debug.Assert(playerState != null);

		uiContainer = Instantiate(UiContainerPrefab);
		uiContainer.SetActive(false);

		rebuildInventory();
	}

	void rebuildInventory () {
		Canvas canvas = uiContainer.GetComponentInChildren<Canvas>();
		for (int i = 0; i < canvas.transform.childCount; i++) {
			Destroy(canvas.transform.GetChild(i).gameObject);
		}

		// Foreach item in inventory...
		int j = 0;
		foreach (Item item in inventory.Contents) {
			Button button = Instantiate(ButtonPrefab);
			button.transform.SetParent(canvas.transform, false);
			button.transform.position = button.transform.position + new Vector3(0f, j * -40f, 0f);

			Item thisItem = item; // Without copying this, every button will get the same argument: the item of the last iteration
			button.onClick.AddListener(() => onItemSelected(thisItem));

			Text label = button.GetComponentInChildren<Text>();
			label.text = item.Name;

			j++;
		}
	}

	void onItemSelected (Item item) {
		if (item.CanUse(gameObject)) {
			item.Use(gameObject);
			inventory.Remove(item);
			closeInventory();
		} else {
			Debug.Log("Selected item " + item.Name + " but cannot currently use.");
		}
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}
		if (Input.GetButtonDown("Inventory")) {
			toggleInventory();
		}
	}

	void toggleInventory () {
		if (uiContainer.activeSelf) {
			closeInventory();
		} else {
			openInventory();
		}
	}

	void openInventory () {
		rebuildInventory();
		playerState.IsInInventory = true;
		uiContainer.SetActive(true);
	}

	void closeInventory () {
		uiContainer.SetActive(false);
		playerState.IsInInventory = false;
	}
}
