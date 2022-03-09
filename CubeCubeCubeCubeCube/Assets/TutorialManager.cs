using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance { get; private set; }
    public static bool hasSeenTutorial = false;

    public bool isShown = false;
    public GameObject removedElements;
    public CanvasGroup persistedElements;
    public CanvasGroup hudElements;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (hasSeenTutorial)
        {
            Destroy(gameObject);
            return;
        }
        
        hasSeenTutorial = true;
        Player.instance.playerControls.enabled = false;
        Player.instance.playerGun.enabled = false;
        Player.instance.enabled = false;
        isShown = true;
    }

    void Update()
    {
        if (!isShown) return;

        hudElements.alpha = 0.3f + (Mathf.Sin(Time.unscaledTime * Mathf.PI * 2) / 2 + 0.5f) * 0.7f;
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Player.instance.playerControls.enabled = true;
            Player.instance.playerGun.enabled = true;
            Player.instance.enabled = true;
            isShown = false;
            hudElements.alpha = 1f;
            Destroy(removedElements);
            
            StartCoroutine(HideTutorialRoutine());
        }
    }

    public IEnumerator HideTutorialRoutine()
    {
        for (float t = 0; t < 3; t += Time.unscaledDeltaTime)
        {
            yield return null;
        }
        
        for (float t = 0; t < 3; t += Time.unscaledDeltaTime)
        {
            persistedElements.alpha = 1f - t / 3f;
            yield return null;
        }
    
        Destroy(gameObject);
    }
}
