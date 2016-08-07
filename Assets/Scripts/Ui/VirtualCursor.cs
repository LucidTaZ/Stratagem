using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class VirtualCursor : MonoBehaviour {
	public GameObject CursorObject; // No prefab please

	Canvas canvas;

	PlayerState playerState;

	void Start () {
		canvas = GetComponentInChildren<Canvas>();
		Debug.Assert(canvas != null);

		if (canvas.GetComponent<BoxCollider>() == null) {
			Debug.LogError("The canvas must have a BoxCollider to receive Physics Events");
		}

		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
	}

	void Update () {
		Vector2 localPosition;
		if (castRay(out localPosition)) {
			moveMouseCursor(localPosition);
			playerState.IsInVirtualConsole = true;
		} else {
			playerState.IsInVirtualConsole = false;
		}
	}

	bool castRay (out Vector2 localPosition) {
		// This method uses the configured VirtualConsoleRaycaster to see if we can interact with the virtual console.
		// If so, it uses a conventional physics raycast to get the actual world point.

		Vector2 screenPosition;
		if (eventSystemHovered(out screenPosition)) {
			Vector3 worldPosition = screenToWorldPosition(screenPosition);
			Vector3 localPosition3D = canvas.transform.InverseTransformPoint(worldPosition);
			localPosition = localPosition3D;
			return true;
		}

		localPosition = new Vector2();
		return false;
	}

	bool eventSystemHovered (out Vector2 screenPosition) {
		List<RaycastResult> hits = new List<RaycastResult>();
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
		VirtualConsoleRaycaster raycaster = GetComponentInChildren<VirtualConsoleRaycaster>();

		raycaster.Raycast(pointer, hits);

		if (hits.Count == 0) {
			screenPosition = new Vector2();
			return false;
		}

		screenPosition = hits[0].screenPosition;
		return true;
	}

	Vector3 screenToWorldPosition (Vector2 screenPosition) {
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		RaycastHit hitInfo;
		if (!Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << canvas.gameObject.layer)) {
			Debug.LogError("Screenposition has no place in world");
		}
		return hitInfo.point;
	}

	void moveMouseCursor (Vector2 localPosition) {
		CursorObject.GetComponent<RectTransform>().localPosition = localPosition;
	}
}
