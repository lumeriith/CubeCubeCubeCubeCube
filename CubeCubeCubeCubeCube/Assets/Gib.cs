using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gib : MonoBehaviour {
    private Material material;
    private Rigidbody rb;
    private void Start()
    {
        material = GetComponent<Renderer>().material;
        rb = GetComponent<Rigidbody>();
        rb.velocity = Random.insideUnitSphere * 12f;
        rb.AddTorque( Random.insideUnitSphere.normalized * 50f);
        float randomEmission = Random.Range(2f, 3.5f);
        material.color = new Color(randomEmission, randomEmission, randomEmission);
        Destroy(gameObject, 4f);
    }
    private void Update()
    {
        Color newColour = material.color;
        newColour.r = Mathf.MoveTowards(newColour.r, 0, Time.deltaTime * 4f);
        newColour.g = Mathf.MoveTowards(newColour.r, 0, Time.deltaTime * 4f);
        newColour.b = Mathf.MoveTowards(newColour.r, 0, Time.deltaTime * 4f);

        material.color = newColour;

        rb.AddForce(Vector3.down * 20 * Time.deltaTime, ForceMode.VelocityChange);
    }

}
