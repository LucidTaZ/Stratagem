using UnityEngine;
using UnityEngine.Networking;

// Usage: place on Player object but disabled. Enable it to go into "placement mode"
public class PlaceStructure : NetworkBehaviour {
	public GameObject Subject;

	Material BlueprintMaterial;
	AudioClip BuildSound;

	BelongsToTeam btt;

	PlayerState playerState;

	const float MAX_DISTANCE = 5f;
	const float MIN_NORMAL_ALIGNMENT = 0.9f; // Minimum dot product of surface normal and "up"

	bool storedDomainVisuals;
	GameObject domainIndicator;

	public bool IsPlacing {
		get { return enabled; }
	}

	void Awake () {
		btt = GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);

		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
		Debug.Assert(playerState != null);

		BlueprintMaterial = Resources.Load<Material>("BlueprintMaterial"); // So we don't have to assign it all the time upon instantiation, but can still leverage the editor to design the material
		BuildSound = Resources.Load<AudioClip>("BuildSound");
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
			if (Input.GetButton("Build")) {
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
		return true;
	}

	[Command]
	public void CmdBuildStructure (NetworkHash128 subjectId, Vector3 position, Quaternion rotation) {
		GameObject subject = ClientScene.prefabs[subjectId];
		GameObject structure = Instantiate(subject);
		structure.transform.position = position;
		structure.transform.rotation = rotation;

		BelongsToTeam childBtt = structure.GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);
		Debug.Assert(childBtt != null);
		childBtt.CopyFrom(btt);

		NetworkServer.Spawn(structure);
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
		Matrix4x4 buildlocationToLocal = Matrix4x4.TRS(position, rotation, Vector3.one);

		foreach (Renderer mr in Subject.GetComponentsInChildren<Renderer>()) {
			MeshFilter filter;
			if ((filter = mr.GetComponent<MeshFilter>()) != null) {
				Mesh mesh = filter.sharedMesh;
				Matrix4x4 meshrendererToWorld = buildlocationToLocal * mr.transform.localToWorldMatrix;
				Graphics.DrawMesh(mesh, meshrendererToWorld, BlueprintMaterial, gameObject.layer, Camera.main);
			}
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
}
