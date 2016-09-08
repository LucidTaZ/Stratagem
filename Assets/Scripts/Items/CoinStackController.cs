using UnityEngine;

public class CoinStackController : MonoBehaviour {
	public ItemIdentifier ItemIdToWatch;

	Inventory inventory;
	CoinStack coinStack;

	void Start () {
		inventory = GetComponentInParent<Inventory>();
		coinStack = GetComponent<CoinStack>();

		Debug.Assert(inventory != null);
		Debug.Assert(coinStack != null);
	}

	void Update () {
		coinStack.Count = inventory.Count(ItemIdToWatch);
	}
}
