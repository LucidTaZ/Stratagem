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
		GameObject subject = Instantiate(Subject);
		subject.transform.position = SpawnLocation.position;
		subject.transform.rotation = SpawnLocation.rotation;
		Spawnable[] spawnables = subject.GetComponents<Spawnable>();
		foreach (Spawnable spawnable in spawnables) {
			spawnable.SetSource(this);
		}
		BelongsToTeam btt;
		if ((btt = GetComponent<BelongsToTeam>()) != null) {
			BelongsToTeam childBtt = subject.AddComponent<BelongsToTeam>();
			childBtt.CopyFrom(btt);
		}
	}

	public void OnSubjectDestroyed (GameObject subject) {
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
			MeshFilter filter;
			if ((filter = mr.GetComponent<MeshFilter>()) != null) {
				Mesh mesh = filter.sharedMesh;
				Gizmos.DrawCube(mesh.bounds.center, mesh.bounds.size * 1.1f);
			}
		}
	}
#endif

	void renderEditorPreview () {
		Matrix4x4 spawnlocationToLocal = Matrix4x4.TRS (SpawnLocation.transform.position, Quaternion.identity, Vector3.one);

		foreach (Renderer mr in Subject.GetComponentsInChildren(typeof (Renderer), true))
		{
			MeshFilter filter;
			if ((filter = mr.GetComponent<MeshFilter>()) != null) {
				Mesh mesh = filter.sharedMesh;
				Matrix4x4 meshrendererToWorld = spawnlocationToLocal * mr.transform.localToWorldMatrix;
				List<Material> materials = new List<Material> (mr.sharedMaterials);
				for (int i = 0; i < materials.Count; i++) {
					Graphics.DrawMesh (mesh, meshrendererToWorld, materials[i], gameObject.layer, null, i);
				}
			}
		}
	}
}
