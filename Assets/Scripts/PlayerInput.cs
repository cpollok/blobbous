using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public PlayerCharacterController controller;
    public Camera mainCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (controller.Dead || controller.NoControl) {
            return;
        }
        Vector3 pos = GetMouseOnPlane();
        controller.Move(pos.x, pos.z);
        if (Input.GetMouseButtonDown(0)) {
            controller.SwingWeapon();
        }
	}

    Vector3 GetMouseOnPlane() {
        Vector3 pos = Input.mousePosition;
        Ray screenRay = mainCamera.ScreenPointToRay(pos);
        Plane ground = new Plane(Vector3.up, controller.transform.position);
        float distToScreen;
        ground.Raycast(screenRay, out distToScreen);
        return screenRay.GetPoint(distToScreen);
    }
}
