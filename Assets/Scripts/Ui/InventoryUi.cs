using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryUi : MonoBehaviour {
	public GameObject UiContainerPrefab;
	public Button ButtonPrefab;

	Inventory inventory;

	GameObject uiContainer;

	Behaviour[] componentsToSuspend;

	void Start () {
		inventory = GetComponent<Inventory>();
		Debug.Assert(inventory != null);
		Debug.Assert(UiContainerPrefab != null);
		Debug.Assert(ButtonPrefab != null);

		uiContainer = Instantiate(UiContainerPrefab);
		uiContainer.SetActive(false);

		rebuildInventory();
		registerComponentsToSuspend();
	}

	void rebuildInventory () {
		Canvas canvas = uiContainer.GetComponentInChildren<Canvas>();
		for (int i = 0; i < canvas.transform.childCount; i++) {
			Destroy(canvas.transform.GetChild(i).gameObject);
		}

		// Foreach item in inventory...
		int j = 0;
		foreach (Item item in inventory.Contents.Items) {
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

	void registerComponentsToSuspend () {
		componentsToSuspend = new Behaviour[]{
			GetComponent<FirstPersonController>(),
			GetComponent<Shoot>(),
			GetComponent<PlaceStructure>(),
		};
	}

	void onItemSelected (Item item) {
		item.Use(gameObject);
		inventory.Remove(item);
		closeInventory();
	}

	void Update () {
		if (Input.GetButtonDown("Inventory")) {
			registerComponentsToSuspend(); // Since components may change dynamically, such as PlaceStructure
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
		suspendComponents();
		freeCursor();
		uiContainer.SetActive(true);
	}

	void closeInventory () {
		uiContainer.SetActive(false);
		// Cursor does not have to be locked upon closing the inventory, because the FirstPersonController does that
		freeComponents();
	}

	void freeCursor () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void suspendComponents () {
		foreach (Behaviour component in componentsToSuspend) {
			if (component != null) {
				component.enabled = false;
			}
		}
	}

	void freeComponents () {
		foreach (Behaviour component in componentsToSuspend) {
			if (component != null) {
				component.enabled = true;
			}
		}
	}
}
