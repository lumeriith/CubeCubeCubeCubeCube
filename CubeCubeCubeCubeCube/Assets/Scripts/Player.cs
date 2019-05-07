using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    static public Player instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
            }
            return _instance;
        }
    }
    static private Player _instance;



    public float shield = 100f;
    public float maxShield = 100f;
    public float shieldChargePerSecond = 15f;
    public float shieldChargeCooldown = 0f;
    public float health = 50f;
    public float maxHealth = 50f;
    public bool isDead = false;
    


    [Header("Internals")]
    public PlayerControls playerControls;
    public PlayerGun playerGun;
    public PlayerZaWarudo playerZaWarudo;
    public Rigidbody rb;


    private Material material;

    private void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        playerGun = GetComponent<PlayerGun>();
        playerZaWarudo = GetComponent<PlayerZaWarudo>();
        rb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
    }


    private void Update()
    {
        AddEmission(-Time.deltaTime * 7.5f);
        Color newColour = material.color;
        newColour.r = Mathf.MoveTowards(newColour.r, 1, Time.deltaTime * 2);
        newColour.g = Mathf.MoveTowards(newColour.g, 1, Time.deltaTime * 2);
        newColour.b = Mathf.MoveTowards(newColour.b, 1, Time.deltaTime * 2);

        material.color = newColour;
        shieldChargeCooldown = Mathf.MoveTowards(shieldChargeCooldown, 0, Time.deltaTime);
        if(shieldChargeCooldown == 0)
        {
            shield = Mathf.MoveTowards(shield, maxShield, Time.deltaTime * shieldChargePerSecond);
        }

        if (health <= 0 && playerControls.enabled)
        {
            GameManager.instance.OnPlayerDeath();
            playerControls.enabled = false;
            playerGun.enabled = false;
            isDead = true;
            rb.constraints = RigidbodyConstraints.None;
            rb.velocity += Random.insideUnitSphere.normalized * 30f;
            rb.AddTorque(Random.insideUnitSphere.normalized * 50f, ForceMode.VelocityChange);
            material.color = new Color(0, 0, 0);
            enabled = false;
            

        }
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;
        shieldChargeCooldown = 2.5f;
        if(amount > 90)
        {
            General.instance.sfx.Play("Explosion");
        }
        else
        {
            General.instance.sfx.Play("DamageSelf");
        }
        
        if(shield > amount)
        {
            shield -= amount;
            SetEmission(1f);
        }
        else
        {
            health -= amount - shield;
            shield = 0;
            SetEmission(.7f);
            material.color = new Color(1, 0, 0);
        }
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
