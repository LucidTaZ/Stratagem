using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkControlConstraint : NetworkBehaviour {
	public GameObject LocalModel;
	public GameObject RemoteModel;

	void Start () {
		Debug.Assert(LocalModel != null);
		Debug.Assert(RemoteModel != null);
		Debug.Assert(LocalModel != RemoteModel);

		if (LocalModel.activeSelf) {
			Debug.LogWarning("Make sure LocalModel is not active by default");
		}
		if (!RemoteModel.activeSelf) {
			Debug.LogWarning("Make sure RemoteModel is active by default");
		}
	}

	override public void OnStartAuthority () {
		GetComponent<FirstPersonController>().enabled = true;
		GetComponent<AttachCamera>().enabled = true;
		GetComponent<InventoryUi>().enabled = true;
		GetComponent<Shoot>().enabled = true;

		LocalModel.SetActive(true);
		RemoteModel.SetActive(false);

		GetComponent<AttachCamera>().PerformAttach();
	}
}
