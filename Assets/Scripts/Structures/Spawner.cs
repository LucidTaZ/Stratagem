using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

// When adding networking, see https://docs.unity3d.com/Manual/UNetSpawning.html

[ExecuteInEditMode]
public class Spawner : MonoBehaviour {

	public GameObject Subject;
	public Transform SpawnLocation;

	public float CooldownSeconds = 10;
	public bool SpawnImmediately = true;

	public bool AllowMultipleItems = true;

	float cooldownLeft;
	bool itemExists = false;

	void Start () {
		if (SpawnImmediately) {
			cooldownLeft = 0;
		} else {
			cooldownLeft = CooldownSeconds;
		}
	}

	void Update () {
#if UNITY_EDITOR
		if (EditorApplication.isPlaying) {
			gameModeUpdate();
		} else {
			editModeUpdate();
		}
#else
		gameModeUpdate();
#endif
	}

	void gameModeUpdate () {
		if (!AllowMultipleItems && itemExists) {
			return;
		}
		cooldownLeft -= Time.deltaTime;
		if (cooldownLeft <= 0) {
			PerformSpawn();
			cooldownLeft = CooldownSeconds;
			itemExists = true;
		}
	}

	void editModeUpdate () {
		renderEditorPreview();
	}

	void PerformSpawn () {
		GameObject item = Instantiate(Subject);
		item.transform.parent = SpawnLocation;
		item.transform.localPosition = Vector3.zero;
		Lootable lootable = item.GetComponent<Lootable>();
		if (lootable != null) {
			lootable.SetSource(this);
		}
	}

	public void OnItemPickup (Lootable item) {
		itemExists = false;
	}

#if UNITY_EDITOR
	void OnDrawGizmos () {
		drawGizmos (new Color (0, 0, 0, 0));
	}
	void OnDrawGizmosSelected () {
		drawGizmos (new Color (0, 0, 1, .2f));
	}
	void drawGizmos (Color color) {
		if (EditorApplication.isPlaying) {
			return;
		}
		Gizmos.color = color;
		Matrix4x4 spawnlocationToLocal = Matrix4x4.TRS (SpawnLocation.transform.position, Quaternion.identity, Vector3.one);
		foreach (Renderer mr in Subject.GetComponentsInChildren(typeof (Renderer), true))
		{
			Matrix4x4 meshrendererToWorld = spawnlocationToLocal * mr.transform.localToWorldMatrix;
			Gizmos.matrix = meshrendererToWorld;
			Mesh mesh = mr.GetComponent<MeshFilter>().sharedMesh;
			Gizmos.DrawCube(mesh.bounds.center, mesh.bounds.size * 1.1f);
		}
	}
#endif

	void renderEditorPreview () {
		Matrix4x4 spawnlocationToLocal = Matrix4x4.TRS (SpawnLocation.transform.position, Quaternion.identity, Vector3.one);

		foreach (Renderer mr in Subject.GetComponentsInChildren(typeof (Renderer), true))
		{
			Mesh mesh = mr.GetComponent<MeshFilter>().sharedMesh;
			Matrix4x4 meshrendererToWorld = spawnlocationToLocal * mr.transform.localToWorldMatrix;
			List<Material> materials = new List<Material> (mr.sharedMaterials);
			for (int i = 0; i < materials.Count; i++) {
				Graphics.DrawMesh (mesh, meshrendererToWorld, materials[i], gameObject.layer, null, i);
			}
		}
	}
}
