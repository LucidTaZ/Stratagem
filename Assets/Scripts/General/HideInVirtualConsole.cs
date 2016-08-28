using UnityEngine;

public class HideInVirtualConsole : MonoBehaviour {
	Renderer[] renderers;
	PlayerState playerState;

	bool currentlyHiding;

	void Awake () {
		renderers = GetComponentsInChildren<Renderer>();
		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
	}

	void Start () {
		setDisplay();
	}

	void Update () {
		toggleDisplayIfNeeded();
	}

	void toggleDisplayIfNeeded () {
		if (currentlyHiding != playerState.IsInVirtualConsole) {
			currentlyHiding = playerState.IsInVirtualConsole;
			setDisplay();
		}
	}

	void setDisplay () {
		foreach (Renderer renderer in renderers) {
			renderer.enabled = !currentlyHiding;
		}
	}
}
