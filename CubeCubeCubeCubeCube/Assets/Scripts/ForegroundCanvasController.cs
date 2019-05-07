using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForegroundCanvasController : MonoBehaviour {

    static public ForegroundCanvasController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ForegroundCanvasController>();
            }
            return _instance;
        }
    }
    static private ForegroundCanvasController _instance;

    private Vector3 lastVelocity;
    private Vector3 cv;
    private Vector3 lastCameraPosition;

    private Image currentAmmo;
    private Image crosshair;
    private Image shieldIcon;
    private Image currentShield;
    private Image currentHealth;
    private Transform cameraTransform;

    private Image zaWarudoCharge;
    private Image zaWarudoHand;

    public Material zaWarudoReady;
    public Material zaWarudoCharging;

    private Player player;
    private Material scoreMaterial;

    private Text score;
    private Text scoreBackdrop;

    private Text distance;

    private Text stars;
    public void SetScoreEmission(float amount)
    {
        Color eColour = scoreMaterial.GetColor("_EmissionColor");
        eColour.r = amount;
        eColour.g = amount;
        eColour.b = amount;
        scoreMaterial.SetColor("_EmissionColor", eColour);
    }

    public void AddScoreEmission(float amount)
    {
        Color eColour = scoreMaterial.GetColor("_EmissionColor");
        eColour.r += amount;
        eColour.g += amount;
        eColour.b += amount;
        if (eColour.r < 1) eColour.r = 1;
        if (eColour.g < 1) eColour.g = 1;
        if (eColour.b < 1) eColour.b = 1;

        scoreMaterial.SetColor("_EmissionColor", eColour);
    }

    void Start()
    {
        
        currentAmmo = transform.FindDeepChild("Current Ammo").GetComponent<Image>();
        crosshair = transform.FindDeepChild("Crosshair").GetComponent<Image>();
        currentHealth = transform.FindDeepChild("Current Health").GetComponent<Image>();
        currentShield = transform.FindDeepChild("Current Shield").GetComponent<Image>();
        shieldIcon = transform.FindDeepChild("Shield Icon").GetComponent<Image>();
        zaWarudoCharge = transform.FindDeepChild("ZaWarudo Charge").GetComponent<Image>();
        zaWarudoHand = transform.FindDeepChild("ZaWarudo Hand").GetComponent<Image>();
        score = transform.FindDeepChild("Score").GetComponent<Text>();
        scoreMaterial = score.material;
        scoreBackdrop = transform.FindDeepChild("Score Backdrop").GetComponent<Text>();
        distance = transform.FindDeepChild("Distance").GetComponent<Text>();
        stars = transform.FindDeepChild("Stars").GetComponent<Text>();
        player = Player.instance;
        cameraTransform = CameraRigController.instance.GetComponentInChildren<Camera>().transform;
        lastCameraPosition = cameraTransform.position;

    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        AddScoreEmission(-Time.deltaTime * 4.5f);
        if (!player.isDead)
        {
            currentAmmo.fillAmount = player.playerGun.ammo / player.playerGun.maxAmmo;
            crosshair.enabled = player.playerGun.ammo == player.playerGun.maxAmmo;
            lastCameraPosition = Vector3.SmoothDamp(lastCameraPosition, cameraTransform.position, ref cv, .2f);
            transform.localPosition = (cameraTransform.position - lastCameraPosition) * 5f;
            currentShield.fillAmount = player.shield / player.maxShield;
            currentHealth.fillAmount = player.health / player.maxHealth;
            Color newShieldIconColour = shieldIcon.color;
            newShieldIconColour.a = .5f + (player.shield / player.maxShield) * .5f;
            shieldIcon.color = newShieldIconColour;
            shieldIcon.enabled = player.shield > 0;

            zaWarudoHand.enabled = player.playerZaWarudo.currentCooldown == 0;
            zaWarudoHand.transform.rotation = Quaternion.Euler(0, 0, player.playerZaWarudo.currentDuration/ player.playerZaWarudo.maxDuration * -360f);
            zaWarudoCharge.fillAmount = 1 - player.playerZaWarudo.currentCooldown / player.playerZaWarudo.cooldown;
            zaWarudoCharge.material = player.playerZaWarudo.currentCooldown == 0 ? zaWarudoReady : zaWarudoCharging;
            scoreBackdrop.text = GameManager.instance.score.ToString("#,#");
            score.text = scoreBackdrop.text;
            stars.text = new string('★', GameManager.instance.difficulty);


            distance.text = ((int)(GameManager.instance.travelDistance)).ToString("#,#0") + "m";


        }
        else
        {
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            
        }
            


    }
}
