using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
public class CameraRigController : MonoBehaviour {

    static public CameraRigController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CameraRigController>();
            }
            return _instance;
        }
    }
    static private CameraRigController _instance;
    private Vector3 offset;
    private Vector3 smoothedPosition;
    public Camera cam;
    private Vector3 cv;
    private float shakeAmount;
    public float smoothTime = .2f;
    public TimePostProcessingTransition timePostProcessingTransition;
    public PostProcessingProfile slowedProfile;
    public PostProcessingProfile defaultProfile;
    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        timePostProcessingTransition = GetComponentInChildren<TimePostProcessingTransition>();
        offset = transform.position - Player.instance.transform.position;


    }
    public void ApplyCameraShake(float amount)
    {
        shakeAmount = Mathf.Max(amount, shakeAmount);
    }

    

    void Update()
    {
        Vector3 targetPosition = offset + Player.instance.transform.position + Vector3.ClampMagnitude(Player.instance.rb.velocity, 8f) * -.20f;
        float delta = Player.instance.transform.position.z - GameManager.instance.wallOfDeath.transform.position.z;
        Quaternion targetRotation = Quaternion.Euler(0, Mathf.Clamp(30 - delta * 4, 0, 30), 0);



        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 60 * Time.deltaTime);
        

        if(targetPosition.y < GameManager.instance.worldBottomBoundary)
        {
            targetPosition.y = GameManager.instance.worldBottomBoundary;
        }
        smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref cv, smoothTime);
        transform.position = smoothedPosition + Random.insideUnitSphere.normalized * shakeAmount;
        shakeAmount = Mathf.MoveTowards(shakeAmount, 0, Time.deltaTime * 4f);
        
        if(timePostProcessingTransition.futureProfile == slowedProfile && Player.instance.playerZaWarudo.currentDuration == 0)
        {
            timePostProcessingTransition.futureProfile = defaultProfile;
            timePostProcessingTransition.StartTransition();
        }
        else if (Player.instance.playerZaWarudo.currentDuration != 0 && timePostProcessingTransition.futureProfile != slowedProfile)
        {
            timePostProcessingTransition.futureProfile = slowedProfile;
            timePostProcessingTransition.StartTransition();
        }
    }
}
