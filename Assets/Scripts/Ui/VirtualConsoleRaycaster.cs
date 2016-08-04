using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VirtualConsoleRaycaster : GraphicRaycaster {

	public float MaxDistance;

	public override void Raycast (PointerEventData eventData, List<RaycastResult> resultAppendList) {
		List<RaycastResult> newList = new List<RaycastResult>();
		base.Raycast(eventData, newList);
		foreach (RaycastResult rcr in newList) {
			if (rcr.distance <= MaxDistance) {
				resultAppendList.Add(rcr);
			}
		}
	}
}
