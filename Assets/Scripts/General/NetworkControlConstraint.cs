using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class NetworkControlConstraint : NetworkBehaviour {
	override public void OnStartAuthority () {
		GetComponent<FirstPersonController>().enabled = true;
		GetComponent<AttachCamera>().enabled = true;
		GetComponent<InventoryUi>().enabled = true;
		GetComponent<Shoot>().enabled = true;

		GetComponent<AttachCamera>().PerformAttach();
	}
}
