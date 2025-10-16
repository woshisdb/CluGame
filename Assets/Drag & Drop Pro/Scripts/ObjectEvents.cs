using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	// Object Setting
	ObjectSettings OS;
	// DDM Game Object
	DragDropManager DDM;

	void Awake () {
		// Getting the Object Setting that assigned to this GameObject
		OS = GetComponent<ObjectSettings> (); 
		// Getting DDM GameObject
		DDM = FindObjectOfType<DragDropManager>();
	}

	public void OnPointerDown (PointerEventData eventData) {
		if (!OS.OnReturning) {
			if (DDM.TargetPlatform == DragDropManager.Platforms.PC) {
				// for PC
				if (eventData.button == PointerEventData.InputButton.Left) {
					OS.PointerDown ("User", null);
				}
			} else {
				// for Mobile
				if (eventData.pointerId == 0) {
					OS.PointerDown ("User", null);
				}
			}
		}
	}

	public void OnPointerUp (PointerEventData eventData) {
		if (!OS.OnReturning) {
			if (DDM.TargetPlatform == DragDropManager.Platforms.PC)
			{
				// for PC
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					OS.PointerUp("User");
				}
			}
			else
			{
				// for Mobile
				if (eventData.pointerId == 0)
                {
					OS.PointerUp("User");
				}
			}
		}
	}
}
