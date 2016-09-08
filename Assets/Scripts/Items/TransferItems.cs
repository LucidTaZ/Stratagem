using UnityEngine;
using UnityEngine.Networking;

// Automatically transfers contents to another destination
public class TransferItems : NetworkBehaviour {
	public ItemIdentifier[] Items;

	public float Bandwidth = Mathf.Infinity; // Number of items per second
	float secondsPerItem;
	float nextTransferTime;

	Inventory own;
	Inventory target;

	BelongsToTeam btt;

	void Awake () {
		own = GetComponent<Inventory>();
		Debug.Assert(own != null);

		btt = GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);

		secondsPerItem = 1.0f / Bandwidth;
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
			while (own.Contains(itemIdentifier) && nextTransferTime <= Time.time) {
				Item item = own.Take(itemIdentifier);
				target.Add(item);
				nextTransferTime = Time.time + secondsPerItem;
			}
		}
	}

	void setTeamInventory () {
		target = FindGameobjects.FindTeamInventory(btt.team);
	}
}
