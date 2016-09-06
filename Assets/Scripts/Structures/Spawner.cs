using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

// When adding networking, see https://docs.unity3d.com/Manual/UNetSpawning.html

[ExecuteInEditMode]
public class Spawner : NetworkBehaviour {

	public GameObject Subject;
	public Transform SpawnLocation;

	public float CooldownSeconds = 10;
	public bool SpawnImmediately = true;

	public int ItemLimit = 1; // Set to 0 for limitless

	float cooldownLeft;
	int itemsExisting = 0;

	KnowsDomain knowsDomain;

	void Awake () {
		knowsDomain = GetComponent<KnowsDomain>();
	}

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
		if (!hasAuthority) {
			return;
		}
		if (itemsExisting >= ItemLimit && ItemLimit != 0) {
			return;
		}
		cooldownLeft -= Time.deltaTime;
		if (cooldownLeft <= 0) {
			PerformSpawn();
			cooldownLeft = CooldownSeconds;
			itemsExisting++;
		}
	}

	void editModeUpdate () {
		renderEditorPreview();
	}

	void PerformSpawn () {
		GameObject subject = (GameObject)Instantiate(Subject, SpawnLocation.position, SpawnLocation.rotation);

		Spawnable spawnable = subject.GetComponent<Spawnable>();
		if (spawnable != null) {
			spawnable.SetSource(this);
		}
		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		BelongsToTeam childBtt = subject.GetComponent<BelongsToTeam>();
		if (btt != null && childBtt != null) {
			childBtt.CopyFrom(btt);
		}

		if (knowsDomain != null) {
			FindTarget findTarget = subject.GetComponent<FindTarget>();
			if (findTarget != null) {
				findTarget.SetDomain(knowsDomain.Domain);
			}
		}

		NetworkServer.Spawn(subject);
	}

	public void OnSubjectDestroyed (GameObject subject) {
		itemsExisting--;
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
