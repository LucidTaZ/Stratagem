using UnityEngine;
using UnityEngine.Networking;

public class PurchaseHandler : NetworkBehaviour {
	public PurchaseableItem[] Assortment;

	Inventory teamInventory;

	void Awake () {
		teamInventory = GetComponent<Inventory>();
		Debug.Assert(teamInventory != null);
		Debug.Assert(Assortment != null);
	}

	public void Purchase (PurchaseableItem purchaseableItem) {
		ItemIdentifier zeny = new ItemIdentifier("Zeny"); // TODO: Make settable via PurchasableItem

		if (!hasAuthority) {
			Debug.LogWarning("Trying to purchase something without authority!");
		} else {
			Debug.Log("Purchasing something with authority!");
		}

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
		NetworkServer.Spawn(loot);
	}
}
