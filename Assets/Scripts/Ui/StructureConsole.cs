using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StructureConsole : MonoBehaviour {
	public Button ButtonPrefab;
	public GameObject CursorObject; // No prefab please

	public float MaxDistance;
	float sqrMaxDistance;

	Canvas canvas;

	const string LAYER_NAME = "World UI";

	static Vector2 VirtualScreenDimension = new Vector2(800, 600);

	void Start () {
		canvas = GetComponentInChildren<Canvas>();
		Debug.Assert(canvas != null);

		if (canvas.GetComponent<BoxCollider>() == null) {
			Debug.LogError("The canvas must have a BoxCollider to receive Physics Events");
		}
		if (canvas.gameObject.layer != LayerMask.NameToLayer(LAYER_NAME)) {
			Debug.LogError("The canvas must have the World UI layer to receive raycasts");
		}

		sqrMaxDistance = MaxDistance * MaxDistance;

		buildContents();
	}

	void buildContents () {
		Button button = Instantiate(ButtonPrefab);
		button.transform.SetParent(canvas.transform, false);

		button.onClick.AddListener(OnPurchaseButtonClicked);

		Text label = button.GetComponentInChildren<Text>();
		label.text = "Text set via script";
	}

	void Update () {
		if (playerIsCloseEnough()) {
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // TODO: Unify with other raycast
			Vector2 virtualScreenPosition;
			if (castRay(ray, out virtualScreenPosition)) {
				moveMouseCursor(virtualScreenPosition);
				if (Input.GetButtonDown("Shoot")) {
					Debug.Log("Click on " + virtualScreenPosition);
					// TODO: Maybe find a way to fake an event through the official GUI event system? GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<EventSystem>()...
					// Otherwise we will have to do some coordinate checking ourselves...
					// ExecuteEvent.Execute(new Event(...)); ?
				}
				// TODO: Disable crosshair once it exists
			} else {
				// TODO: Enable crosshair
			}
		}
	}

	bool playerIsCloseEnough () {
		return (Camera.main.transform.position - canvas.transform.position).sqrMagnitude < sqrMaxDistance;
	}

	bool castRay (Ray ray, out Vector2 virtualScreenPosition) {
		// Lower-left corner is (0,0), upper-right corder is (800,600)
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, MaxDistance, 1 << LayerMask.NameToLayer(LAYER_NAME))) {
			Vector3 worldPosition = hitInfo.point;
			Vector3 localPosition3D = canvas.transform.InverseTransformPoint(worldPosition);
			Vector2 localPosition2D = localPosition3D;
			virtualScreenPosition = VirtualScreenDimension / 2 + localPosition2D;
			return true;
		}
		virtualScreenPosition = new Vector2();
		return false;
	}

	void moveMouseCursor (Vector2 virtualScreenPosition) {
		Vector2 unityLocalPosition = virtualScreenPosition - VirtualScreenDimension / 2; // Can be confusing: the editor shows something else because of the anchor point
		CursorObject.GetComponent<RectTransform>().localPosition = unityLocalPosition;
	}

	void OnPurchaseButtonClicked () {
		Debug.Log("Purchase button clicked on " + gameObject.name);
	}
}
