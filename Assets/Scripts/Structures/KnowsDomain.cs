using UnityEngine;

public class KnowsDomain : MonoBehaviour {
	public TargetDomain Domain;

	void Start () {
		Domain.Center = transform.position;

		BelongsToTeam btt = GetComponent<BelongsToTeam>();
		if (btt != null) {
			Domain.FriendlyTeam = btt.team;
		}
	}
}
