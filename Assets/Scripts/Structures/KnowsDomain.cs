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
		initializeRangeIndicator(RangeIndicator);
	}

	public GameObject GetRangeIndicatorBeforeConstruction () {
		// In this method, the structure is not actually built yet, so we miss some information such as team
		GameObject indicator = Instantiate(RangeIndicator);
		initializeRangeIndicatorTransform(indicator);
		return indicator;
	}

	void initializeRangeIndicator (GameObject indicator) {
		// TODO: Maybe drawing a line is better than having a big cylinder.
		// http://answers.unity3d.com/questions/31949/position-a-circle-under-my-character.html
		// http://docs.unity3d.com/Manual/class-Projector.html
		// http://docs.unity3d.com/Manual/class-LineRenderer.html
		initializeRangeIndicatorTransform(indicator);
		initializeRangeIndicatorColor(indicator);
	}

	void initializeRangeIndicatorTransform (GameObject indicator) {
		indicator.transform.position = new Vector3(
			Domain.Center.x,
			Domain.Center.y + indicator.transform.position.y,
			Domain.Center.z
		);
		Projector projector = indicator.GetComponentInChildren<Projector>();
		if (projector != null) {
			projector.orthographicSize = Domain.Range;
		} else {
			// GameObject-based indicator, simply scale
			indicator.transform.localScale = new Vector3(
				Domain.Range * 2,
				indicator.transform.localScale.y,
				Domain.Range * 2
			);
		}
	}

	void initializeRangeIndicatorColor (GameObject indicator) {
		foreach (Renderer renderer in indicator.GetComponents<Renderer>()) {
			Material material = renderer.material;
			setColorRGBOnly(material, "_Color", Domain.FriendlyTeam.color);
			material.SetColor("_EmissionColor", Color.Lerp(Color.black, Domain.FriendlyTeam.color, 0.1f));
		}
		foreach (Projector projector in indicator.GetComponentsInChildren<Projector>(true)) {
			Material material = Instantiate(projector.material); // Projectors don't instantiate them themselves, they all share the reference. We could optimize this by having only one instance per team, instead of an instance per structure
			setColorRGBOnly(material, "_Color", Domain.FriendlyTeam.color);
			projector.material = material;
		}
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
