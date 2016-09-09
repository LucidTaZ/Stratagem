using UnityEngine;
using UnityEngine.Networking;

// "Sucks" items from things that trigger it
public class AcceptItems : NetworkBehaviour {
	public ItemIdentifier[] Items;

	Inventory inventory;
	BelongsToTeam parentBtt;
	Team team;

	void Awake () {
		GameObject parent = transform.parent.gameObject;
		Debug.Assert(parent != null);

		parentBtt = parent.GetComponent<BelongsToTeam>();
		Debug.Assert(parentBtt != null);

		inventory = GetComponentInParent<Inventory>();
		Debug.Assert(inventory != null); // Cannot suck items when we have no place to put it
	}

	void Start () {
		team = parentBtt.team;
	}

	void OnTriggerEnter (Collider other) {
		if (!NetworkServer.active) {
			return;
		}
		Inventory otherInventory = other.GetComponent<Inventory>();
		BelongsToTeam otherBtt = other.GetComponent<BelongsToTeam>();

		if (otherBtt != null && otherBtt.team.IsFriendsWith(team) && otherInventory != null) {
			foreach (ItemIdentifier itemIdentifier in Items) {
				take(otherInventory, itemIdentifier);
			}
		}
	}

	void take (Inventory otherInventory, ItemIdentifier itemIdentifier) {
		while (otherInventory.Contains(itemIdentifier) && inventory.HasSpace()) {
			Item item = otherInventory.Take(itemIdentifier);
			inventory.Add(item);
		}
	}
}
