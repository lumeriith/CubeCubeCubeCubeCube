using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCloudCanvasController : MonoBehaviour {
    private Image deathCloud;
    private void Start()
    {
        deathCloud = transform.Find("Death Cloud").GetComponent<Image>();
    }
    void Update () {

        deathCloud.enabled = GameManager.instance.wallOfDeath.transform.position.z > CameraRigController.instance.cam.transform.position.z;

	}
}
