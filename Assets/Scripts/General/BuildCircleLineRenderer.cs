using UnityEngine;

public class BuildCircleLineRenderer : MonoBehaviour {
	[Range(1, 64)]
	public int Vertices;

	void Start () {
		LineRenderer lr = GetComponent<LineRenderer>();
		Debug.Assert(lr != null);
		lr.SetVertexCount(Vertices);
		lr.SetPositions(generatePositions());
	}

	Vector3[] generatePositions () {
		Vector3[] result = new Vector3[Vertices];
		float step = 360.0f / (Vertices - 1); // Subtract one to place the last equal to the first
		for (int i = 0; i < Vertices; i++) {
			result[i] = Quaternion.AngleAxis(i * step, Vector3.up) * new Vector3(0, 0, 0.5f);
		}
		return result;
	}
}
