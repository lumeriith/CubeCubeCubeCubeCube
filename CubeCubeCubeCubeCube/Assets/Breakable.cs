using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    Health health;
    private void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update () {
        if (health.health <= 0)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            GetComponent<Collider>().enabled = false;
            Destroy(this);
            Destroy(gameObject, 3f);
        }
	}
}
