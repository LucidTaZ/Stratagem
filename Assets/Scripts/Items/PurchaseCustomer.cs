using UnityEngine;
using UnityEngine.Networking;

// The reason this proxy script exists is because commands need to be sent from the Player object in order to respect the authority. The client doesn't have authority over the structure purchase console.
// More info: https://www.reddit.com/r/Unity3D/comments/4bwjps/unet_trying_to_send_command_for_object_without/
public class PurchaseCustomer : NetworkBehaviour {
	public void Purchase (PurchaseableItem purchaseableItem, PurchaseHandler handler) {
		CmdPurchase(purchaseableItem, handler.gameObject.GetComponent<NetworkIdentity>().netId);
	}

	[Command]
	public void CmdPurchase (PurchaseableItem purchaseableItem, NetworkInstanceId handlerId) {
		GameObject handlerObject = ClientScene.FindLocalObject(handlerId);
		Debug.Assert(handlerObject != null);
		PurchaseHandler handler = handlerObject.GetComponent<PurchaseHandler>();
		Debug.Assert(handler != null);
		handler.Purchase(purchaseableItem);
	}
}
