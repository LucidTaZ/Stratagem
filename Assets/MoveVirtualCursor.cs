using UnityEngine;
using System.Collections;

public class MoveVirtualCursor : MonoBehaviour {
	public GameObject CursorObject; // No prefab please

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseMoved () {
		Debug.Log("Mouse over " + gameObject.name);
	}
}
