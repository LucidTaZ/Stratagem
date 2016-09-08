using UnityEngine;

[ExecuteInEditMode]
public class CoinStack : MonoBehaviour {
	public int Count;
	public GameObject ItemModel;

	const int MAX_INSTANCES = 100;
	const float COIN_HEIGHT = 0.05f;
	const float COIN_WIDTH = 0.25f;

	int nInstantiated = 0;
	GameObject[] instantiated = new GameObject[MAX_INSTANCES];

	void Update () {
		while (nInstantiated > Count && nInstantiated > 0) {
			// Remove until satisfied
			deactivateTop();
		}
		while (nInstantiated < Count && nInstantiated < MAX_INSTANCES) {
			// Instantiate more
			instantiateOrActivateNext();
		}
	}

	void instantiateOrActivateNext () {
		if (instantiated[nInstantiated] != null) {
			instantiated[nInstantiated].SetActive(true);
		} else {
			instantiated[nInstantiated] = instantiateNew();
		}
		nInstantiated++;
	}

	GameObject instantiateNew () {
		GameObject result = Instantiate(ItemModel);
		result.transform.parent = transform;
		result.transform.localPosition = getPosition(nInstantiated);
		return result;
	}

	static Vector3 getPosition (int coinIndex) {
		int[] stackSizes = { 22, 17, 24, 19, 18 };

		int totalPreviousStacks = 0;
		int totalWithThisStack = 0;
		int stackNumber = 0;

		foreach (int stackSize in stackSizes) {
			totalWithThisStack += stackSize;
			if (coinIndex < totalWithThisStack) {
				// Add the coin to this stack
				// Place stacks next to each other in x
				// Stack coins on top of each other in y
				return new Vector3(
					COIN_WIDTH * stackNumber,
					(coinIndex - totalPreviousStacks) * COIN_HEIGHT,
					0f
				);
			}
			totalPreviousStacks += stackSize;
			stackNumber++;
		}

		Debug.LogError("This code should be unreachable, stack sizes do not sum up to MAX_INSTANCES!");
		return Vector3.zero;
	}

	void deactivateTop () {
		nInstantiated--;
		instantiated[nInstantiated].SetActive(false);
	}
}
