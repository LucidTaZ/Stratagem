using UnityEngine;

public struct MeshAndTransform {
	public Mesh mesh;
	public Matrix4x4 transform;

	public MeshAndTransform (Mesh mesh, Matrix4x4 transform) {
		this.mesh = mesh;
		this.transform = transform;
	}
}
