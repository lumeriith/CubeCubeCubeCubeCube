using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    public Transform entryPoint;
    public Transform exitPoint;
    [Header("Internals")]
    public Vector3 targetPosition;
	void Start () {
        entryPoint.gameObject.SetActive(false);
        exitPoint.gameObject.SetActive(false);
	}
	
}
