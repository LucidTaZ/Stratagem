using UnityEngine;
using UnityEngine.UI;
using System;

public class HudBoxController : MonoBehaviour {
	public Sprite BoxSprite;
	public Sprite BoxSelectedSprite;

	public Image IconImage;
	public Text LabelText;

	Image boxImage;
	Action actionOnActivation;

	void Awake () {
		boxImage = GetComponent<Image>();
		Debug.Assert(boxImage != null);

		Debug.Assert(IconImage != null);
		Debug.Assert(LabelText != null);
	}

	public void SetSelected (bool selected) {
		boxImage.sprite = selected ? BoxSelectedSprite : BoxSprite;
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

	// For testing:
	bool selected = true;
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			SetSelected(selected = !selected);
			SetIcon(ItemFactory.Instance().GetIcon(new ItemIdentifier("Worker Spawner")));
			SetText("Worker Spawner");
		}
	}
}
