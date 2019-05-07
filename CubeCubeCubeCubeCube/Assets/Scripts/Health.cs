using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float health = 100f;
    private Material material;


    public void TakeDamage(float damage)
    {
        health -= damage;
        SetEmission(1f);
        
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        AddEmission(-Time.deltaTime * 4.5f);
    }
    
    private void SetEmission(float amount)
    {
        Color eColour = material.GetColor("_EmissionColor");
        eColour.r = amount;
        eColour.g = amount;
        eColour.b = amount;
        material.SetColor("_EmissionColor", eColour);
    }

    private void AddEmission(float amount)
    {
        Color eColour = material.GetColor("_EmissionColor");
        eColour.r += amount;
        eColour.g += amount;
        eColour.b += amount;
        if (eColour.r < 0) eColour.r = 0;
        if (eColour.g < 0) eColour.g = 0;
        if (eColour.b < 0) eColour.b = 0;

        material.SetColor("_EmissionColor", eColour);
    }
}
