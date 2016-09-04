using UnityEngine;

public class ForbidStructurePlacement : MonoBehaviour {
	public float Radius;

	public bool CanPlace (Vector3 position) {
		ForbidStructurePlacement[] fsps = FindObjectsOfType<ForbidStructurePlacement>();
		foreach (ForbidStructurePlacement fsp in fsps) {
			if (fsp != this) {
				if (!fsp.CanPlace(position, this)) {
					return false;
				}
			}
		}
		return true;
	}

	bool CanPlace (Vector3 otherPosition, ForbidStructurePlacement other) {
		float minDistance = Radius + other.Radius;
		return (otherPosition - transform.position).sqrMagnitude > minDistance * minDistance;
	}
}
