using UnityEngine;
using UnityEngine.Networking;

// Automatically transfers contents to another destination
public class TransferItems : NetworkBehaviour {
	public ItemIdentifier[] Items;

	Inventory own;
	Inventory target;

	BelongsToTeam btt;

	void Awake () {
		own = GetComponent<Inventory>();
		Debug.Assert(own != null);

		btt = GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);
	}

	void FixedUpdate () {
		if (!NetworkServer.active) {
			return;
		}
		if (target == null) {
			// Can happen if we have team structures preset in the level, without any players (or at least hearts) present.
			setTeamInventory();
			return;
		}
		foreach (ItemIdentifier itemIdentifier in Items) {
			while (own.Contains(itemIdentifier)) {
				Item item = own.Take(itemIdentifier);
				target.Add(item);
			}
		}
	}

	void setTeamInventory () {
		target = FindGameobjects.FindTeamInventory(btt.team);
	}
}
