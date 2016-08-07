using UnityEngine;

public class Crosshair : MonoBehaviour {
	PlayerState playerState;

	void Start () {
		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
		Debug.Assert(playerState != null);
	}

	void Update () {
		GetComponent<Canvas>().enabled = playerState.CanShoot;
	}
}
