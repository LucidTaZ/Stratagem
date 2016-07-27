using UnityEngine;

public class Hitpoints : MonoBehaviour {

	public int Initial = 5;

	int current;

	void Start () {
		current = Initial;
	}

	public void Decrease (int delta) {
		current -= delta;
		if (current <= 0) {
			Die();
		}
	}

	void Die () {
		Destroy(gameObject);
	}
}
