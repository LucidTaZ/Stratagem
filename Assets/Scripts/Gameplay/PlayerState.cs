using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerState : MonoBehaviour {
	public bool IsInOverlayMenu;
	public bool IsInVirtualConsole;

	public bool IsInInventory {
		get { return IsInOverlayMenu; }
		set { IsInOverlayMenu = value; }
	}

	public bool CanMouselook {
		get { return !IsInOverlayMenu; }
	}

	public bool CanShoot {
		get { return !IsInOverlayMenu && !IsInVirtualConsole; }
	}

	public bool CanPlaceStructures {
		get { return !IsInOverlayMenu && !IsInVirtualConsole; }
	}

	GameObject player;

	void ensurePlayerReference () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
	}

	void Update () {
		ensurePlayerReference();
		if (IsInOverlayMenu) {
			freeCursor();
			disablePlayerController();
		} else {
			// Cursor does not have to be locked, because the FirstPersonController does that
			enablePlayerController();
		}
	}

	void freeCursor () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void disablePlayerController () {
		if (player != null) {
			Behaviour fpc = player.GetComponent<FirstPersonController>();
			fpc.enabled = false;
		}
	}

	void enablePlayerController () {
		if (player != null) {
			Behaviour fpc = player.GetComponent<FirstPersonController>();
			fpc.enabled = true;
		}
	}
}
