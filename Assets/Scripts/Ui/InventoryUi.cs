using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class InventoryUi : NetworkBehaviour {
	public GameObject HudboxPrefab;

	Inventory inventory;
	PlayerState playerState;
	GameObject uiContainer;
	ItemFactory itemFactory;

	int selectionIndex;
	List<GameObject> instantiatedHudBoxes = new List<GameObject>();

	void Start () {
		if (!hasAuthority) {
			return;
		}

		inventory = GetComponent<Inventory>();
		Debug.Assert(inventory != null);
		inventory.AddMutationCallback(rebuildInventory);

		GameObject playerStateObject = GameObject.FindGameObjectWithTag("PlayerState");
		Debug.Assert(playerStateObject != null);
		playerState = PlayerState.Instance();

		uiContainer = GameObject.FindGameObjectWithTag("HUD");
		Debug.Assert(uiContainer != null);

		itemFactory = ItemFactory.Instance();
		Debug.Assert(itemFactory != null);

		rebuildInventory();
	}

	void rebuildInventory () {
		foreach (GameObject hudBox in instantiatedHudBoxes) {
			Destroy(hudBox);
		}
		instantiatedHudBoxes.Clear();

		// Foreach item in inventory...
		int j = 0;
		foreach (Item.Stack stack in inventory.GetStackedContents()) {
			GameObject hudBox = Instantiate(HudboxPrefab);
			hudBox.transform.SetParent(uiContainer.transform, false);
			hudBox.transform.localPosition += new Vector3(0f, j * -50f, 0f);

			Item thisItem = stack.ContainedItem; // Without copying this, every button will get the same argument: the item of the last iteration

			HudBoxController hbc = hudBox.GetComponent<HudBoxController>();
			Debug.Assert(hbc != null);
			if (stack.Count > 1) {
				hbc.SetText(stack.ContainedItem.Name + " x" + stack.Count);
			} else {
				hbc.SetText(stack.ContainedItem.Name);
			}
			hbc.SetIcon(itemFactory.GetIcon(new ItemIdentifier(stack.ContainedItem.Name)));
			hbc.SetAction(() => tryToUse(thisItem));
			hbc.SetSelected(j == selectionIndex);

			instantiatedHudBoxes.Add(hudBox);

			j++;
		}

		// Reposition current selection in case the inventory shrunk
		if (selectionIndex >= instantiatedHudBoxes.Count && instantiatedHudBoxes.Count > 0) {
			selectionIndex = instantiatedHudBoxes.Count - 1;
			enableSelectionVisual();
		}
	}

	void scrollUp () {
		if (instantiatedHudBoxes.Count == 0) {
			return;
		}
		disableSelectionVisual();
		selectionIndex--;
		if (selectionIndex < 0) {
			selectionIndex += instantiatedHudBoxes.Count;
		}
		enableSelectionVisual();
	}

	void scrollDown () {
		if (instantiatedHudBoxes.Count == 0) {
			return;
		}
		disableSelectionVisual();
		selectionIndex = (selectionIndex + 1) % instantiatedHudBoxes.Count;
		enableSelectionVisual();
	}

	void disableSelectionVisual () {
		getCurrentHudBoxController().SetSelected(false);
	}

	void enableSelectionVisual () {
		getCurrentHudBoxController().SetSelected(true);
	}

	HudBoxController getCurrentHudBoxController () {
		GameObject hudBox = instantiatedHudBoxes[selectionIndex];
		return hudBox.GetComponent<HudBoxController>();
	}

	void activateSelectedItem () {
		if (instantiatedHudBoxes.Count == 0) {
			return;
		}
		getCurrentHudBoxController().Activate();
	}

	void tryToUse (Item item) {
		if (item.CanUse(gameObject)) {
			item.Use(gameObject);
			inventory.Remove(item);
		}
	}

	void Update () {
		if (!hasAuthority) {
			return;
		}

		if (Input.GetButtonDown("Select Item") && playerState.CanActivateInventoryItem) {
			activateSelectedItem();
		}

		if (Input.GetAxis("List Scroll") < -0.5 && playerState.CanScrollInventory) {
			scrollDown();
		} else if (Input.GetAxis("List Scroll") > 0.5 && playerState.CanScrollInventory) {
			scrollUp();
		}
	}
}
