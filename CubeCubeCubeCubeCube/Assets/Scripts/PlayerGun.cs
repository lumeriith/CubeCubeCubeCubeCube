using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

    private Player player;

    public float ammo = 100f;
    public float maxAmmo = 100f;
    public float ammoChargePerSecond = 150f;
    public float shootKnockbackVelocity = 2f;
    public float powerShootKnockbackVelocity = 5f;
    public LayerMask whatStopsBullet;
    public GameObject trail;
    
    [Header("Internals")]
    public float ammoChargeCooldown = 0f;
    public float gunCooldown = 0f;

    private GameObject laser;
    public Transform muzzle;
    private GameObject muzzleflash;



    private void Awake()
    {
        player = GetComponent<Player>();
        muzzle = transform.FindDeepChild("Muzzle");
        muzzleflash = transform.FindDeepChild("Muzzleflash").gameObject;
        laser = GameObject.Find("Laser");
        muzzleflash.SetActive(false);
    }

    void Update () {
        if (ammoChargeCooldown == 0 && player.playerControls.isGrounded)
        {
            
            if(ammo!= maxAmmo)
            {
                ammo = Mathf.MoveTowards(ammo, maxAmmo, ammoChargePerSecond * Time.deltaTime);
                if(ammo == maxAmmo)
                {
                    General.instance.sfx.Play("Ding");
                }
            }
            
        }

        if (Input.GetButton("Fire1") && gunCooldown == 0)
        {
            if(ammo == maxAmmo)
            {

                ammo -= 25f;
                ammoChargeCooldown = .4f;
                gunCooldown = .30f;
                PowerPush();
                Shoot(50, .5f);
                player.playerControls.isJumping = false;
                General.instance.sfx.Play("Sniper");
            } else if(ammo >= 10f)
            {
                ammo -= 10f;
                ammoChargeCooldown = .4f;
                gunCooldown = .20f;
                Push();
                Shoot(15, .2f);
                player.playerControls.isJumping = false;
                General.instance.sfx.Play("Pistol");

                
            }
        }


        ammoChargeCooldown = Mathf.MoveTowards(ammoChargeCooldown, 0, Time.deltaTime);
        gunCooldown = Mathf.MoveTowards(gunCooldown, 0, Mathf.Max(Time.deltaTime, Time.unscaledDeltaTime));
        
        RaycastHit hitInfo;

        laser.SetActive(false); // Lasers are deactivated until its protruding is fixed.
        /*
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hitInfo, 75f, whatStopsBullet, QueryTriggerInteraction.Collide))
        {


            laser.transform.rotation = muzzle.rotation;
            laser.transform.localScale = new Vector3(.15f, .15f, hitInfo.distance / 2f);
            laser.transform.position = muzzle.position + laser.transform.forward * (hitInfo.distance / 2f);

        }
        else
        {

            laser.transform.rotation = muzzle.rotation;
            laser.transform.localScale = new Vector3(.15f, .15f, 75 / 2f);
            laser.transform.position = muzzle.position + laser.transform.forward * (75 / 2f);
        }
        */
    }
        

    public void Shoot(float damage, float size)
    {
        RaycastHit hitInfo;

        GameManager.instance.totalShots += 1;
        if (Physics.Raycast(muzzle.position - muzzle.forward * .8f, muzzle.forward, out hitInfo, 100f, whatStopsBullet, QueryTriggerInteraction.Collide))
        {

            GameObject newTrail = Instantiate(trail, muzzle.position, muzzle.rotation);

            newTrail.transform.localScale = new Vector3(size, size, hitInfo.distance/2+1);
            newTrail.transform.position += newTrail.transform.forward * (hitInfo.distance/2f+1);

            Health health = hitInfo.collider.GetComponent<Health>();
            if(health != null)
            {
                General.instance.sfx.PlayDelayed("DamageOther", .13f);
                health.TakeDamage(damage);
                GameManager.instance.SpawnGib(hitInfo.point + newTrail.transform.forward*0.5f, (int)(damage/3));
            }
            else
            {
                GameManager.instance.missedShots += 1;
            }
            GameManager.instance.SpawnBulletHit(hitInfo.point);
        }
        else
        {
            GameObject newTrail = Instantiate(trail, muzzle.position, muzzle.rotation);

            newTrail.transform.localScale = new Vector3(size, size, 75);
            newTrail.transform.position += newTrail.transform.forward * 75;
        }

        StopCoroutine("MuzzleflashRoutine");
        StartCoroutine("MuzzleFlashRoutine");

    }

    IEnumerator MuzzleFlashRoutine()
    {
        muzzleflash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleflash.SetActive(false);
    }

    public void Push()
    {



        float velocityModifier = 1f;
        if (player.playerControls.isGrounded)
        {
            velocityModifier = .33f;
        }

        Vector3 newVelocity = player.rb.velocity;
        newVelocity.y *= 0.85f;
        newVelocity += -muzzle.forward * shootKnockbackVelocity * velocityModifier;
        
        player.rb.velocity = newVelocity;
        
        CameraRigController.instance.ApplyCameraShake(.5f);

        
    }

    public void PowerPush()
    {



        float velocityModifier = 1f;
        if (player.playerControls.isGrounded)
        {
            velocityModifier = .33f;
        }

        Vector3 newVelocity = player.rb.velocity;
        Vector3 deltaVelocity = -muzzle.forward * powerShootKnockbackVelocity * velocityModifier;
        if(deltaVelocity.z < 0)
        {
            deltaVelocity.z *= .5f;
        }
        newVelocity.y *= 0.3f;
        newVelocity += deltaVelocity;

        player.rb.velocity = newVelocity;

        CameraRigController.instance.ApplyCameraShake(1f);
    }

    

}
