using UnityEngine;

// "Sucks" items from things that trigger it
public class AcceptItems : MonoBehaviour {
	public ItemIdentifier[] Items;

	Inventory inventory;
	Team team;

	void Start () {
		team = transform.parent.gameObject.GetComponent<BelongsToTeam>().team;

		inventory = GetComponent<Inventory>();
		Debug.Assert(inventory != null); // Cannot suck items when we have no place to put it
	}

	void OnTriggerEnter (Collider other) {
		Inventory otherInventory = other.GetComponent<Inventory>();
		BelongsToTeam otherBtt = other.GetComponent<BelongsToTeam>();

		if (otherBtt != null && otherBtt.team.IsFriendsWith(team) && otherInventory != null) {
			foreach (ItemIdentifier itemIdentifier in Items) {
				take(otherInventory, itemIdentifier);
			}
		}
	}

	void take (Inventory otherInventory, ItemIdentifier itemIdentifier) {
		while (otherInventory.Contains(itemIdentifier)) {
			Item item = otherInventory.Take(itemIdentifier);
			inventory.Add(item);
		}
	}
}
