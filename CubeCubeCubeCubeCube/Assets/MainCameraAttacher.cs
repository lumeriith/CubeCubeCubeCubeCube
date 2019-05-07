using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraAttacher : MonoBehaviour {
    void Update()
    {
        transform.position = Camera.main.transform.position;
    }
}
