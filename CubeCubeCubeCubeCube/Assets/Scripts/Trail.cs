using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {


    private Vector3 originalScale;
    private void Start()
    {
        Destroy(transform.parent.gameObject, .5f);
        originalScale = transform.localScale;
        
    }
    private void Update()
    {

        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.MoveTowards(newScale.x, 0, Time.deltaTime * 2.5f * originalScale.x);
        newScale.z = Mathf.MoveTowards(newScale.z, 0, Time.deltaTime * 2.5f * originalScale.z);
        transform.localScale = newScale;

    }
}
