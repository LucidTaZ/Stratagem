using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	NavMeshAgent nav;
	GameObject player;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		nav = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
		nav.SetDestination(player.transform.position);
	}
}
