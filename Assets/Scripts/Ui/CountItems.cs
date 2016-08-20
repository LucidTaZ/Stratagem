using UnityEngine;
using UnityEngine.UI;

public class CountItems : MonoBehaviour {
	public ItemIdentifier Item;

	Inventory teamInventory;

	Text textField;
	string format;

	void Start () {
		textField = GetComponent<Text>();
		Debug.Assert(textField != null);
		format = textField.text;
	}

	void Update () {
		if (teamInventory == null) {
			setTeam();
			return;
		}
		int count = teamInventory.Count(Item);
		textField.text = string.Format(format, count);
	}

	void setTeam () {
		GameObject localPlayer = FindGameobjects.FindLocalPlayerObject();
		if (localPlayer == null) {
			return;
		}
		BelongsToTeam btt = localPlayer.GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);
		teamInventory = FindGameobjects.FindTeamInventory(btt.team);
	}
}
