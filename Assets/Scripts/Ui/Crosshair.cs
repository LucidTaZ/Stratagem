using UnityEngine;

public class Crosshair : MonoBehaviour {
	PlayerState playerState;

	void Start () {
		playerState = PlayerState.Instance();
	}

	void Update () {
		GetComponent<Canvas>().enabled = playerState.CanShoot;
	}
}
