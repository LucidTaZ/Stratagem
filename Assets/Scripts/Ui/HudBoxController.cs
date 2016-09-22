using UnityEngine;
using UnityEngine.UI;
using System;

public class HudBoxController : MonoBehaviour {
	public GameObject SelectionIndicator;

	public Image IconImage;
	public Text LabelText;

	Image boxImage;
	Action actionOnActivation;

	void Awake () {
		boxImage = GetComponent<Image>();
		Debug.Assert(boxImage != null);

		Debug.Assert(SelectionIndicator != null);
		Debug.Assert(IconImage != null);
		Debug.Assert(LabelText != null);
	}

	public void SetSelected (bool selected) {
		SelectionIndicator.SetActive(selected);
	}

	public void SetIcon (Sprite icon) {
		IconImage.sprite = icon;
	}

	public void SetText (string text) {
		LabelText.text = text;
	}

	public void SetAction (Action action) {
		actionOnActivation = action;
	}

	public void Activate () {
		actionOnActivation.Invoke();
	}
}
