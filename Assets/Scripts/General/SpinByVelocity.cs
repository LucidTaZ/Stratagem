using UnityEngine;

public class SpinByVelocity : MonoBehaviour {
	public Vector3 Axis = Vector3.right;
	public float Rate = 90; // (degrees/s) / (m/s) = degrees / m

	Vector3 previousPosition;

	void Update () {
		//float ds = (transform.position - previousPosition).magnitude; // Counts any direction, always spins wheels forwards
		float ds = Vector3.Cross(transform.position - previousPosition, Axis).magnitude; // Counts forward & back direction, and should spin wheels backwards when driving backwards
		previousPosition = transform.position;
		float v = ds / Time.deltaTime;
		float omega = (v * Rate);
		transform.Rotate(Axis, omega * Time.deltaTime);
	}
}
