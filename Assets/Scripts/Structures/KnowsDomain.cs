using UnityEngine;

public class KnowsDomain : MonoBehaviour {
	public TargetDomain Domain;
	public GameObject RangeIndicator;

	PlayerState playerState;

	bool showingRanges;

	void Awake () {
		Debug.Assert(RangeIndicator != null);
	}

	void Start () {
		Domain.Center = transform.position;

		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		Debug.Assert(btt != null);
		Domain.FriendlyTeam = btt.team;

		playerState = GameObject.FindGameObjectWithTag("PlayerState").GetComponent<PlayerState>();
		initializeRangeIndicator();
	}

	public MeshAndTransform GetRangeIndicatorBeforeConstruction () {
		// In this method, the structure is not actually built yet, so we miss some information such as team
		Renderer mr = RangeIndicator.GetComponent<Renderer>();
		Debug.Assert(mr != null);
		MeshFilter filter = mr.GetComponent<MeshFilter>();
		Debug.Assert(filter != null);

		Mesh mesh = filter.sharedMesh;
		Matrix4x4 meshrendererToWorld = Matrix4x4.TRS(
			Domain.Center,
			Quaternion.identity,
			new Vector3(Domain.Range * 2, RangeIndicator.transform.localScale.y, Domain.Range * 2)
		);

		return new MeshAndTransform(mesh, meshrendererToWorld);
	}

	void initializeRangeIndicator () {
		// TODO: Maybe drawing a line is better than having a big cylinder.
		// http://answers.unity3d.com/questions/31949/position-a-circle-under-my-character.html
		// http://docs.unity3d.com/Manual/class-Projector.html
		// http://docs.unity3d.com/Manual/class-LineRenderer.html
		initializeRangeIndicatorTransform();
		initializeRangeIndicatorColor();
	}

	void initializeRangeIndicatorTransform () {
		RangeIndicator.transform.position = Domain.Center;
		RangeIndicator.transform.localScale = new Vector3(
			Domain.Range * 2,
			RangeIndicator.transform.localScale.y,
			Domain.Range * 2
		);
	}

	void initializeRangeIndicatorColor () {
		Material material = RangeIndicator.GetComponent<MeshRenderer>().material;
		setColorRGBOnly(material, "_Color", Domain.FriendlyTeam.color);
		material.SetColor("_EmissionColor", Color.Lerp(Color.black, Domain.FriendlyTeam.color, 0.1f));
	}

	void setColorRGBOnly (Material material, string propertyName, Color color) {
		material.SetColor(
			propertyName,
			new Color(
				color.r,
				color.g,
				color.b,
				material.GetColor(propertyName).a
			)
		);
	}

	void Update () {
		if (playerState.ShouldShowTacticalInfo != showingRanges) {
			showingRanges = playerState.ShouldShowTacticalInfo;
			setRangeIndicatorEnabled(showingRanges);
		}
	}

	void setRangeIndicatorEnabled (bool enabled) {
		RangeIndicator.SetActive(enabled);
	}
}
