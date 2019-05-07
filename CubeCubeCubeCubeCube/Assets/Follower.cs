using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
    public Transform following;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        transform.position = following.position;

    }
}
