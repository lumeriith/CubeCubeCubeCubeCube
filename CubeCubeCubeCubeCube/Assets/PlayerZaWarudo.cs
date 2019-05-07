using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
public class PlayerZaWarudo : MonoBehaviour {

    public float currentCooldown = 0f;
    public float cooldown = 4.5f;

    public float maxDuration = 5f;
    public float currentDuration = 0f;

    private Player player;
    private float desiredTimescale = 1f;
    public float timescaleChangeSpeed = 3f;
    public float regularTimescale = 1f;
    public float slowedTimescale = .1f;


    public void MultiplyPitchOfAudioSources(float multiplier)
    {
        foreach(AudioSource a in FindObjectsOfType<AudioSource>())
        {
            a.pitch *= multiplier;
        }
    }

    private void Start()
    {
        player = Player.instance;
    }

    void Update () {
        if(currentCooldown != 0)
        {
            currentCooldown = Mathf.MoveTowards(currentCooldown, 0, Time.unscaledDeltaTime);
            if(currentCooldown == 0)
            {
                General.instance.sfx.Play("ClockReady");
            }
        }
        
        if(Input.GetButton("Fire2") && currentCooldown == 0 && currentDuration < maxDuration)
        {
            if(currentDuration == 0)
            {
                CameraRigController.instance.ApplyCameraShake(1f);

                General.instance.sfx.Play("ClockTicks");
                General.instance.sfx.Play("SlowDown");
                MultiplyPitchOfAudioSources(.5f);
            }

            
            currentDuration += Time.unscaledDeltaTime;

        } else if (currentDuration >= maxDuration || (!Input.GetButton("Fire2") && currentDuration != 0))
        {
            General.instance.sfx.Stop("ClockTicks");
            General.instance.sfx.Play("SlowUp");
            
            currentCooldown = cooldown;
            currentDuration = 0f;
            MultiplyPitchOfAudioSources(2f);
        }


        desiredTimescale = currentDuration != 0 ? slowedTimescale : regularTimescale;
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, desiredTimescale, Time.unscaledDeltaTime * timescaleChangeSpeed);
	}
}
