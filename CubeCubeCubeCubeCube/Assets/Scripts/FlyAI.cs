using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FlyAI : MonoBehaviour {

    private Player player;
    private Health health;
    private Rigidbody rb;
    private Material material;
    private Vector3 offsetAddition;
    private float birthTime;
    public float maximumSpeed = 40f;
    private Vector3 cv;
    public float accelerationPerSecond = 8f;
    public float idleRotationSpeed = 5f;
    public float deathRotationSpeed = 50f;
    public float deathVelocity = 10f;
    public int scoreGibAmount = 0;
    public LayerMask whatIsGround;
    [Header("Offsets")]
    public Vector3 targetOffset = new Vector3(0, 10, 20);
    public float offsetRandomizeMagnitude = 5f;
    public float randomSwayMagnitude = 2f;
    private Vector3 determinedRandomSphere;

    [Header("Attack")]
    public float ageRequiredToAttack;
    public float ageRandomizeMagnitude;
    private float determinedRandomRange;
    public float attacksPerSecond;
    public UnityEvent attack;
    private float attackCooldown;


    private void Awake()
    {
        birthTime = Time.time;
        player = Player.instance;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        material = GetComponent<Renderer>().material;
        determinedRandomSphere = Random.insideUnitSphere.normalized;
        determinedRandomRange = Random.Range(-1f, 1f);
        determinedRandomSphere.x = 0;
        StartCoroutine(RandomizeOffsetAdditionRoutine());
        
    }

    IEnumerator RandomizeOffsetAdditionRoutine()
    {
        while (true)
        {
            offsetAddition = Random.insideUnitSphere.normalized * randomSwayMagnitude;
            offsetAddition.x = 0;
            yield return new WaitForSeconds(1f);
        }
    }

    void Update () {
        if (health.health <= 0)
        {
            rb.useGravity = true;
            rb.velocity += Random.insideUnitSphere.normalized * deathVelocity;
            rb.AddTorque(Random.insideUnitSphere.normalized * deathRotationSpeed, ForceMode.VelocityChange);
            material.color = new Color(0, 0, 0);
            GameManager.instance.SpawnScoreGib(transform.position, scoreGibAmount);
            GameManager.instance.killCount += 1;
            General.instance.sfx.PlayDelayed("Death", .3f);
            Destroy(this);
            Destroy(gameObject, 5f);

            return;
        }

        Vector3 newPosition = transform.position;
        newPosition.x = 0f;
        transform.position = newPosition;
        rb.transform.LookAt(player.transform.position);
        attackCooldown = Mathf.MoveTowards(attackCooldown, 0, Time.deltaTime);

        Vector3 viewportCoords = CameraRigController.instance.cam.WorldToViewportPoint(transform.position);
        bool isOutOfCamera = false;
        if (viewportCoords.x < 0 || viewportCoords.x > 1 || viewportCoords.y < 0 || viewportCoords.y > 1)
        {
            isOutOfCamera = true;
        }

        if (attackCooldown == 0 && Time.time - birthTime > ageRequiredToAttack + determinedRandomRange * ageRandomizeMagnitude && !isOutOfCamera)
        {

            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, (player.transform.position - transform.position).magnitude, whatIsGround , QueryTriggerInteraction.Ignore))
            {
                attackCooldown = .1f;
            }
            else
            {
                attackCooldown = 1f / attacksPerSecond;
                attack.Invoke();
            }

        }
	}

    private void FixedUpdate()
    {
        
        Vector3 targetVelocity = ((player.transform.position + targetOffset + determinedRandomSphere * offsetRandomizeMagnitude + offsetAddition) - transform.position).normalized * maximumSpeed;
        
        rb.velocity = Vector3.MoveTowards(rb.velocity, targetVelocity, Time.deltaTime * accelerationPerSecond);
        

    }

}
