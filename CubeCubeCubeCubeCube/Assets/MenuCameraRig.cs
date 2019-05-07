using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRig : MonoBehaviour {

    Camera cam;
    public float bpm = 80f;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();

    }
    void Update () {
        transform.Rotate(0, Time.deltaTime * 30f, 0);
        
	}
}
