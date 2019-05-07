using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public bool disableMovement = false;
    private Rigidbody rb;
    private GameObject gun;
    private new Renderer renderer;
    public LayerMask whatIsGround;
    public bool isGrounded;
    public float jumpVelocity = 2f;
    public float stickToGroundVelocity = .5f;
    public float defaultSpeed = 5f;
    public float accelerationPerSecond = 20f;
    public float decelerationPerSecond = 15f;
    public float rotationSpeed = 30f;

    public Vector3 normalGravity;
    public Vector3 gravityWhileJumping;

    public bool isJumping = false;
    public float maxJumpReservationTime = .5f;
    private float jumpReservationTime = 0f;


    public bool GetIsGrounded()
    {
        RaycastHit hitInfo;
        float gap = 0.1f;
        return Physics.Raycast(transform.position, Vector3.down, out hitInfo, renderer.bounds.extents.y + gap, whatIsGround, QueryTriggerInteraction.Ignore) ||
            Physics.Raycast(transform.position + Vector3.forward / 2, Vector3.down, out hitInfo, renderer.bounds.extents.y + gap, whatIsGround, QueryTriggerInteraction.Ignore) ||
            Physics.Raycast(transform.position - Vector3.forward / 2, Vector3.down, out hitInfo, renderer.bounds.extents.y + gap, whatIsGround, QueryTriggerInteraction.Ignore);
        
    }

    public void Jump()
    {
        Vector3 v = rb.velocity;
        v.y = jumpVelocity;
        rb.velocity = v;
        isJumping = true;
        isGrounded = false;
        General.instance.sfx.Play("Jump");
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gun = transform.Find("Gun").gameObject;
        renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {


        isGrounded = GetIsGrounded();
            

        if (Input.GetButtonUp("Jump") && isJumping)
        {
            Vector3 v = rb.velocity;
            v.y *= .6f;
            rb.velocity = v;
            
            isJumping = false;

        }


        if (Input.GetButtonDown("Fire1"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                jumpReservationTime = maxJumpReservationTime;
            }
        }
        else
        {
            if (isGrounded && jumpReservationTime == 0)
            {
                rb.AddForce(stickToGroundVelocity * Vector3.down, ForceMode.VelocityChange);
            }
        }
        
        if (jumpReservationTime != 0)
        {
            if (!Input.GetButton("Jump"))
            {
                jumpReservationTime = 0;
            }
            else
            {
                if (isGrounded)
                {
                    transform.rotation = Quaternion.identity;
                    Jump();
                    jumpReservationTime = 0;
                }
                else
                {
                    jumpReservationTime = Mathf.MoveTowards(jumpReservationTime, 0, Time.deltaTime);
                }
            }
        }
        else
        {
            if (isGrounded)
            {
                rb.AddForce(stickToGroundVelocity * Vector3.down, ForceMode.VelocityChange);
            }
        }


        Vector3 newVelocity = rb.velocity;
        if(newVelocity.z > defaultSpeed)
        {
            newVelocity.z = Mathf.MoveTowards(newVelocity.z, defaultSpeed, decelerationPerSecond * Time.deltaTime);
        }
        else
        {
            newVelocity.z = Mathf.MoveTowards(newVelocity.z, defaultSpeed, accelerationPerSecond * Time.deltaTime);
        }
        

        rb.velocity = newVelocity;

        Vector3 cursorPosition = Input.mousePosition;
        cursorPosition.x /= Screen.width;
        cursorPosition.y /= Screen.height;

        Ray cursorRay = GameManager.instance.mainCamera.ViewportPointToRay(cursorPosition);
        Vector3 aimPosition = cursorRay.origin + cursorRay.direction * (-cursorRay.origin.x) / cursorRay.direction.x;


        
        if (!isGrounded)
        {
            Quaternion targetRotation;
            targetRotation = Quaternion.LookRotation(aimPosition - transform.position, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            gun.transform.rotation = targetRotation;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            Quaternion gunTargetRotation = Quaternion.LookRotation(aimPosition - gun.transform.position, Vector3.up);
            gun.transform.rotation = gunTargetRotation;
        }


        if (!isGrounded && isJumping && rb.velocity.y < 0)
        {
            
            isJumping = false;
        }

        Physics.gravity = isJumping ? gravityWhileJumping : normalGravity;

    }

}
