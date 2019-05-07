using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapAttack : MonoBehaviour {
    public float damage = 60f;
    public GameObject trail;
    public float trailSize = .25f;

    

	public void Attack()
    {
        Vector3 dir = Player.instance.transform.position - transform.position;
        Player.instance.TakeDamage(damage);
        GameObject newTrail = Instantiate(trail, transform.position, Quaternion.LookRotation(dir));

        newTrail.transform.localScale = new Vector3(trailSize, trailSize, dir.magnitude/2);
        newTrail.transform.position += newTrail.transform.forward * dir.magnitude/2;
        GameManager.instance.SpawnGib(Player.instance.transform.position, (int)(damage/3));
        CameraRigController.instance.ApplyCameraShake(Mathf.Min(damage/50,1.5f));
        GameManager.instance.SpawnBulletHit(Player.instance.transform.position + (transform.position - Player.instance.transform.position).normalized/2);
    }
}
