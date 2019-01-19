using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamUtils : MonoBehaviour {

    public GameObject arena;

    private Camera cam;

    // Use this for initialization
    void Start() {
        cam = GetComponent<Camera>();
    }

    public Vector3[] GetScreenBoundsInWorld(){

        return new Vector3[] { };
    }
}
