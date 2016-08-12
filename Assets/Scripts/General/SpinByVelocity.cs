using UnityEngine;

public class SpinByVelocity : MonoBehaviour {
	public Vector3 Axis = Vector3.right;
	public float Rate = 3600;

	float angle;
	Vector3 previousPosition;

	void Update () {
		float velocity = (transform.position - previousPosition).magnitude;
		float dangle = (velocity * Rate * Time.deltaTime) % 360;
		angle = (angle + dangle) % 360;
		transform.Rotate(Axis, dangle);
		previousPosition = transform.position;
	}
}
