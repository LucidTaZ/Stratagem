using UnityEngine;

public class GiveVelocityToAnimator : MonoBehaviour {
	Vector3 previousPosition;

	Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
		Debug.Assert(anim != null);
	}

	void Update () {
		float ds = Vector3.Dot(transform.position - previousPosition, -transform.forward); // Note: transform.forward is turned around because I modeled the units the wrong way around...
		previousPosition = transform.position;

		float v = ds / Time.deltaTime;
		bool isMoving = Mathf.Abs(v) > Mathf.Epsilon;

		anim.SetFloat("velocity", v);
		anim.SetBool("isMoving", isMoving);
	}
}
