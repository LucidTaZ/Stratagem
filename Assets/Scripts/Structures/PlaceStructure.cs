using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlaceStructure : NetworkBehaviour {
	// Usage: place on Player object but disabled. Enable it to go into "placement mode"

	public GameObject Subject;

	Material BlueprintMaterial;
	AudioClip BuildSound;

	BelongsToTeam btt;

	PlayerState playerState;

	const float MAX_DISTANCE = 5f;
	const float MIN_NORMAL_ALIGNMENT = 0.9f; // Minimum dot product of surface normal and "up"

	bool storedDomainVisuals;
	GameObject domainIndicator;

	AudioSource audioSource;

	public bool IsPlacing {
		get { return enabled; }
	}

	void Awake () {
		btt = GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);

		playerState = PlayerState.Instance();

		BlueprintMaterial = Resources.Load<Material>("BlueprintMaterial"); // So we don't have to assign it all the time upon instantiation, but can still leverage the editor to design the material
		BuildSound = Resources.Load<AudioClip>("BuildSound");
	}

	void Start () {
		audioSource = Camera.main.GetComponent<AudioSource>();
		Debug.Assert(audioSource != null);
	}

	void OnEnable () {
		Debug.Assert(Subject != null);
	}

	void disableSelf () {
		Subject = null;
		enabled = false;
		Destroy(domainIndicator);
		storedDomainVisuals = false;
	}

	void Update () {
		Vector3 position;
		Quaternion rotation;
		if (hasAuthority && playerState.CanPlaceStructures && canBuild(out position, out rotation)) {
			if (Input.GetButtonDown("Build")) {
				CmdBuildStructure(Subject.GetComponent<NetworkIdentity>().assetId, position, rotation);
				playSound();
				disableSelf();
			} else {
				renderBlueprint(position, rotation);
			}
		} else if (storedDomainVisuals) {
			// We don't hit the ground, so hide the indicator again
			domainIndicator.SetActive(false);
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

		ForbidStructurePlacement fsp = Subject.GetComponent<ForbidStructurePlacement>();
		if (fsp != null && !fsp.CanPlace(position)) {
			return false;
		}
		return true;
	}

	[Command]
	public void CmdBuildStructure (NetworkHash128 subjectId, Vector3 position, Quaternion rotation) {
		GameObject subject = ClientScene.prefabs[subjectId];
		GameObject structure = (GameObject)Instantiate(subject, position, rotation);

		BelongsToTeam childBtt = structure.GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);
		Debug.Assert(childBtt != null);
		childBtt.CopyFrom(btt);

		NetworkServer.Spawn(structure);
	}

	void playSound () {
		audioSource.clip = BuildSound;
		audioSource.Play();
	}

	void renderBlueprint (Vector3 position, Quaternion rotation) {
		Matrix4x4 buildlocationToLocal = Matrix4x4.TRS(position, rotation, Vector3.one);

		foreach (MeshAndTransform mat in findMeshes(Subject)) {
			Matrix4x4 meshrendererToWorld = buildlocationToLocal * mat.transform;
			Graphics.DrawMesh(mat.mesh, meshrendererToWorld, BlueprintMaterial, gameObject.layer, Camera.main);
		}

		KnowsDomain kd = Subject.GetComponent<KnowsDomain>();
		if (kd != null) {
			if (!storedDomainVisuals) {
				domainIndicator = kd.GetRangeIndicatorBeforeConstruction();
				storedDomainVisuals = true;
			}
			domainIndicator.transform.position = position + new Vector3(0, 0.25f, 0);
			domainIndicator.transform.rotation = rotation;
			domainIndicator.SetActive(true);
		}
	}

	static List<MeshAndTransform> findMeshes (GameObject subject) {
		List<MeshAndTransform> result = new List<MeshAndTransform>();

		foreach (Renderer mr in subject.GetComponentsInChildren<Renderer>()) {
			MeshFilter filter;
			if ((filter = mr.GetComponent<MeshFilter>()) != null) {
				result.Add(new MeshAndTransform(filter.sharedMesh, mr.transform.localToWorldMatrix));
			}
		}

		// Skinned MeshRenderer works differently:
		foreach (SkinnedMeshRenderer mr in subject.GetComponentsInChildren<SkinnedMeshRenderer>()) {
			result.Add(new MeshAndTransform(mr.sharedMesh, mr.transform.localToWorldMatrix));
		}

		return result;
	}
}
