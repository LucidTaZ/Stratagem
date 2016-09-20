using UnityEngine;

public class ShowOnTacticalInfo : MonoBehaviour {
	Renderer[] renderers;
	PlayerState playerState;

	bool currentlyDisplaying;

	void Awake () {
		renderers = GetComponentsInChildren<Renderer>();
		playerState = PlayerState.Instance();
	}

	void Start () {
		setDisplay();
	}
	
	void Update () {
		toggleDisplayIfNeeded();
	}

	void toggleDisplayIfNeeded () {
		if (currentlyDisplaying != playerState.ShouldShowTacticalInfo) {
			currentlyDisplaying = playerState.ShouldShowTacticalInfo;
			setDisplay();
		}
	}

	void setDisplay () {
		foreach (Renderer renderer in renderers) {
			renderer.enabled = currentlyDisplaying;
		}
	}
}
