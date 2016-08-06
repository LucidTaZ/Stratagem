using UnityEngine;
using UnityEngine.UI;

public class StructureConsole : MonoBehaviour {
	public Button ButtonPrefab;

	Canvas canvas;

	void Start () {
		canvas = GetComponentInChildren<Canvas>();
		Debug.Assert(canvas != null);

		buildContents();
	}

	void buildContents () {
		Button button = Instantiate(ButtonPrefab);
		button.transform.SetParent(canvas.transform, false);

		button.onClick.AddListener(OnPurchaseButtonClicked);

		Text label = button.GetComponentInChildren<Text>();
		label.text = "Text set via script";
	}

	void OnPurchaseButtonClicked () {
		Debug.Log("Purchase button clicked on " + gameObject.name);
	}
}
