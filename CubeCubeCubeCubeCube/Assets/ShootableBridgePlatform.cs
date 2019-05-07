using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableBridgePlatform : MonoBehaviour {

    public GameObject axis;
    public Health buttonHealth;
    public float rotationSpeed = 100f;
    // Update is called once per frame
    private void Start()
    {
        axis.transform.Rotate(-90, 0, 0);
        buttonHealth.gameObject.transform.position += Random.Range(0, 12f) * Vector3.up;
    }

    void Update () {
		if(buttonHealth.health <= 0)
        {
            
            axis.transform.rotation = Quaternion.RotateTowards(axis.transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeed);
        }
	}
}
