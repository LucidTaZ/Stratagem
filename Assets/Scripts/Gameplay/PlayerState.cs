using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerState : MonoBehaviour {
	public bool IsInOverlayMenu;
	public bool IsInVirtualConsole;
	public bool IsInVirtualConsoleThisTick; // Flag to signal being in a virtual console, so this is a set-only property that reverts back if no one calls it (Reason to have it is to prevent conflicts with multiple consoles)

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
			player = FindGameobjects.FindLocalPlayerObject();
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

	void LateUpdate () {
		IsInVirtualConsole = IsInVirtualConsoleThisTick;
		IsInVirtualConsoleThisTick = false;
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
