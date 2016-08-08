using UnityEngine;
using UnityEngine.UI;

public class CountItems : MonoBehaviour {
	public ItemIdentifier Item;

	public Inventory TeamInventory;

	Text textField;
	string format;

	void Start () {
		Debug.Assert(TeamInventory != null);

		textField = GetComponent<Text>();
		Debug.Assert(textField != null);
		format = textField.text;
	}

	void Update () {
		int count = TeamInventory.Count(Item);
		textField.text = string.Format(format, count);
	}
}
