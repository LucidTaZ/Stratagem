using UnityEngine;

public class FindGameobjects {
	public static GameObject FindClosest (Vector3 center, string tag, float radius = Mathf.Infinity) {
		GameObject result = null;
		GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);

		float limit = radius*radius;
		float closest = Mathf.Infinity;
		foreach (GameObject go in gos) {
			float distanceSq = (go.transform.position - center).sqrMagnitude;
			if (distanceSq < closest && distanceSq < limit) {
				closest = distanceSq;
				result = go;
			}
		}
		return result;
	}
}
