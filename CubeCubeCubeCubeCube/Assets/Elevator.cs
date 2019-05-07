using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

    Health health;
    GameObject elevationPlatform;
    Vector3 targetPosition;
    private void Start()
    {
        health = GetComponentInChildren<Health>();
        elevationPlatform = transform.Find("Elevation Platform").gameObject;
        targetPosition = Vector3.zero;
        
    }
    private void Update()
    {
        if(health.health <= 0)
        {
            if(targetPosition == Vector3.zero)
            {
                targetPosition = elevationPlatform.transform.position + Vector3.up * 15f;
            }
            elevationPlatform.transform.position = Vector3.MoveTowards(elevationPlatform.transform.position, targetPosition, Time.deltaTime*25f);
        }
    }
}
