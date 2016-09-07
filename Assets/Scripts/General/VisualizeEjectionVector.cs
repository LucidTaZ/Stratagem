using UnityEngine;

[ExecuteInEditMode]
public class VisualizeEjectionVector : MonoBehaviour {
	void OnDrawGizmos () {
		Vector3 localDirection = Vector3.forward;
		Vector3 worldDirection = transform.TransformDirection(localDirection);
		Gizmos.DrawRay(transform.position, worldDirection);
	}
}
