using UnityEngine;
using UnityEngine.Networking;

public class PlaceStructure : NetworkBehaviour {
	public GameObject Subject;

	Material BlueprintMaterial;
	AudioClip BuildSound;

	BelongsToTeam btt;

	PlayerState playerState;
	bool isActuallyLocalPlayer;

	const float MAX_DISTANCE = 5f;
	const float MIN_NORMAL_ALIGNMENT = 0.9f; // Minimum dot product of surface normal and "up"

	void Start () {
		btt = GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);
		Debug.Assert(Subject != null);

		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
		Debug.Assert(playerState != null);

		BlueprintMaterial = Resources.Load<Material>("BlueprintMaterial"); // So we don't have to assign it all the time upon instantiation, but can still leverage the editor to design the material
		BuildSound = Resources.Load<AudioClip>("BuildSound");
	}

	void Update () {
		Vector3 position;
		Quaternion rotation;
		if (hasAuthority && playerState.CanPlaceStructures && canBuild(out position, out rotation)) {
			if (Input.GetButton("Build")) {
				buildStructure(position, rotation);
				Destroy(this); // Destroy this script instance
			} else {
				renderBlueprint(position, rotation);
			}
		}
	}

	bool canBuild (out Vector3 position, out Quaternion rotation) {
		position = Vector3.zero;
		rotation = Quaternion.identity;

		Ray groundRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hitInfo;
		if (!Physics.Raycast(groundRay, out hitInfo, MAX_DISTANCE, 1 << LayerMask.NameToLayer("BuildSurface"))) {
			return false;
		}

		if (Vector3.Dot(hitInfo.normal, Vector3.up) < MIN_NORMAL_ALIGNMENT) {
			return false;
		}

		position = hitInfo.point;
		rotation = transform.rotation;
		return true;
	}

	void buildStructure (Vector3 position, Quaternion rotation) {
		GameObject structure = Instantiate(Subject);
		structure.transform.position = position;
		structure.transform.rotation = rotation;
		NetworkServer.Spawn(structure);

		BelongsToTeam childBtt = structure.AddComponent<BelongsToTeam>();
		childBtt.CopyFrom(btt);

		playSound();
	}

	void playSound () {
		AudioSource audio = GetComponent<AudioSource>();
		if (audio == null) {
			Debug.LogWarning("No audio source found");
		} else {
			audio.clip = BuildSound;
			audio.Play();
		}
	}

	void renderBlueprint (Vector3 position, Quaternion rotation) {
		Matrix4x4 spawnlocationToLocal = Matrix4x4.TRS(position, rotation, Vector3.one);

		foreach (Renderer mr in Subject.GetComponentsInChildren<Renderer>(true)) {
			MeshFilter filter;
			if ((filter = mr.GetComponent<MeshFilter>()) != null) {
				Mesh mesh = filter.sharedMesh;
				Matrix4x4 meshrendererToWorld = spawnlocationToLocal * mr.transform.localToWorldMatrix;
				Graphics.DrawMesh(mesh, meshrendererToWorld, BlueprintMaterial, gameObject.layer, Camera.main);
			}
		}
	}
}
