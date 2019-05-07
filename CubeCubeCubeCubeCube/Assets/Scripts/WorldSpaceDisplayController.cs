using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceDisplayController : MonoBehaviour {

    private Image currentAmmo;
    private Image currentAmmoBack;
    private Image crosshair;
    private Image crosshairBack;

    private Player player;
	void Start () {
        currentAmmo = transform.FindDeepChild("Current Ammo").GetComponent<Image>();
        currentAmmoBack = transform.FindDeepChild("Current Ammo Back").GetComponent<Image>();
        crosshair = transform.FindDeepChild("Crosshair").GetComponent<Image>();
        crosshairBack = transform.FindDeepChild("Crosshair Back").GetComponent<Image>();
        player = Player.instance;
	}
	
	// Update is called once per frame
	void LateUpdate () {

        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.x /= Screen.width;
        cursorPosition.y /= Screen.height;

        Ray cursorRay = GameManager.instance.mainCamera.ViewportPointToRay(cursorPosition);
        Vector3 aimPosition = cursorRay.origin + cursorRay.direction * (-cursorRay.origin.x) / cursorRay.direction.x;

        transform.position = aimPosition;
        transform.rotation = player.playerGun.muzzle.rotation;
        currentAmmo.fillAmount = player.playerGun.ammo / player.playerGun.maxAmmo;
        currentAmmoBack.fillAmount = currentAmmo.fillAmount;
        crosshair.enabled = player.playerGun.ammo == player.playerGun.maxAmmo;
        crosshairBack.enabled = crosshair.enabled;
        if (player.isDead)
        {
            gameObject.SetActive(false);
        }
    }
}
