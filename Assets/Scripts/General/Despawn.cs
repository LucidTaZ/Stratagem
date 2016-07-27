using UnityEngine;

public class Despawn : MonoBehaviour {

	public float TimeToLive;

	void Start () {
		Destroy(gameObject, TimeToLive);
	}
}
