using UnityEngine;

public class PurchaseDropZone : MonoBehaviour {
	public Rect Zone;

	public Vector3 SampleWorldLocation () {
		float x = Random.Range(Zone.xMin, Zone.xMax);
		float z = Random.Range(Zone.yMin, Zone.yMax);
		Vector3 localPosition = new Vector3(x, 0f, z);
		Vector3 worldPosition = transform.TransformPoint(localPosition);
		return worldPosition;
	}
}
