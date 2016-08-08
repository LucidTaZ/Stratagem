using UnityEngine;

// Automatically transfers contents to another destination
public class TransferItems : MonoBehaviour {
	public ItemIdentifier[] Items;

	Inventory own;
	Inventory target;

	void Start () {
		own = GetComponent<Inventory>();
		Debug.Assert(own != null);

		target = GameObject.FindGameObjectWithTag("ResourceHolder").GetComponent<Inventory>();
		Debug.Assert(target != null);
	}

	void FixedUpdate () {
		foreach (ItemIdentifier itemIdentifier in Items) {
			while (own.Contains(itemIdentifier)) {
				Item item = own.Take(itemIdentifier);
				target.Add(item);
			}
		}
	}
}
