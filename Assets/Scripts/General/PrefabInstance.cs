using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using System.Collections.Generic;

// Based on: http://framebunker.com/blog/poor-mans-nested-prefabs/

[ExecuteInEditMode]
public class PrefabInstance : MonoBehaviour
{
	public GameObject prefab;

#if UNITY_EDITOR
	// Struct of all components. Used for edit-time visualization and gizmo drawing
	public struct Thingy {
		public Mesh mesh;
		public Matrix4x4 matrix;
		public List<Material> materials;
	}

	[System.NonSerializedAttribute] public List<Thingy> things = new List<Thingy> ();

	void OnValidate () {
		things.Clear();
		if (enabled) {
			Rebuild(prefab, Matrix4x4.identity);
		}
	}

	void OnEnable () {
		things.Clear();
		if (enabled) {
			Rebuild(prefab, Matrix4x4.identity);
		}
	}

	void Rebuild (GameObject source, Matrix4x4 instanceToRoot) {
		if (!source) {
			return;
		}

		Matrix4x4 worldToInstance = Matrix4x4.TRS(-source.transform.position, Quaternion.identity, Vector3.one);
		Matrix4x4 worldToRoot = worldToInstance * instanceToRoot;

		foreach (Renderer mr in source.GetComponentsInChildren(typeof (Renderer), true)) {
			Matrix4x4 meshrendererToWorld = mr.transform.localToWorldMatrix;
			Matrix4x4 meshrendererToRoot = worldToRoot * meshrendererToWorld; // Because A^-1 * B^-1 = (B*A)^-1
			things.Add(new Thingy () {
				mesh = mr.GetComponent<MeshFilter>().sharedMesh,
				matrix = meshrendererToRoot,
				materials = new List<Material>(mr.sharedMaterials)
			});
		}

		foreach (PrefabInstance pi in source.GetComponentsInChildren(typeof (PrefabInstance), true)) {
			if (pi.enabled && pi.gameObject.activeSelf) {
				Matrix4x4 childToWorld = pi.transform.localToWorldMatrix;
				Matrix4x4 childToRoot = childToWorld * worldToRoot;
				Rebuild(pi.prefab, childToRoot);
			}
		}
	}

	// Editor-time-only update: Draw the meshes so we can see the objects in the scene view
	void Update () {
		if (EditorApplication.isPlaying) {
			return;
		}
		Matrix4x4 rootToWorld = transform.localToWorldMatrix;
		foreach (Thingy t in things) {
			Matrix4x4 meshrendererToRoot = t.matrix;
			Matrix4x4 meshrendererToWorld = rootToWorld * meshrendererToRoot;
			for (int i = 0; i < t.materials.Count; i++) {
				Graphics.DrawMesh(t.mesh, meshrendererToWorld, t.materials[i], gameObject.layer, null, i);
			}
		}
	}

	// Picking logic: Since we don't have gizmos.drawmesh, draw a bounding cube around each thingy
	void OnDrawGizmos () {
		DrawGizmos (new Color (0, 0, 0, 0));
	}
	void OnDrawGizmosSelected () {
		DrawGizmos (new Color (0, 0, 1, .2f));
	}
	void DrawGizmos (Color color) {
		if (EditorApplication.isPlaying) {
			return;
		}
		Gizmos.color = color;
		Matrix4x4 rootToWorld = transform.localToWorldMatrix;
		foreach (Thingy t in things) {
			Matrix4x4 meshrendererToRoot = t.matrix;
			Matrix4x4 meshrendererToWorld = rootToWorld * meshrendererToRoot;
			Gizmos.matrix = meshrendererToWorld;
			Gizmos.DrawCube(t.mesh.bounds.center, t.mesh.bounds.size);
		}
	}

	public static void BakeAllInstancesWithUndo () {
		// Undo is important! The custom build script performs an Undo operation. Without it, our spawned instances remain in the scene!
		foreach (PrefabInstance pi in UnityEngine.Object.FindObjectsOfType (typeof (PrefabInstance))) {
			BakeInstanceWithUndo(pi);
		}
	}

	// Baking stuff: Copy in all the referenced objects into the scene on play or build
	[PostProcessScene(-2)]
	public static void OnPostprocessScene () { 
		// There are three use cases which must not be confused:
		// 1. Editor preview: meshes are rendered using Update()
		// 2. Editor play mode or load level during gameplay: this method
		// 3. Custom build: the OnCustomBuild() method, the (custom) build pipeline bakes the NavMesh to ensure the PrefabInstances are properly carved. Its changes are undone by the build pipeline
		foreach (PrefabInstance pi in UnityEngine.Object.FindObjectsOfType (typeof (PrefabInstance))) {
			BakeInstance(pi);
		}
	}

	public static void BakeInstanceWithUndo (PrefabInstance pi) {
		BakeInstance(pi, true);
	}

	public static void BakeInstance (PrefabInstance pi, bool registerUndo = false) {
		if (!pi.prefab || !pi.enabled) {
			return;
		}

		if (registerUndo) {
			Undo.RegisterCompleteObjectUndo(pi, "Bake instance for NavMesh");
		}

		pi.enabled = false;
		GameObject go = PrefabUtility.InstantiatePrefab(pi.prefab) as GameObject;

		if (registerUndo) {
			Undo.RegisterCreatedObjectUndo(go, "Instantiate Prefab");
		}
		pi.prefab = null;

		Quaternion rot = go.transform.localRotation;
		Vector3 scale = go.transform.localScale;
		go.transform.parent = pi.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = scale;
		go.transform.localRotation = rot;

		foreach (PrefabInstance childPi in go.GetComponentsInChildren<PrefabInstance>()) {
			BakeInstance(childPi, registerUndo);
		}
	}

#endif
}