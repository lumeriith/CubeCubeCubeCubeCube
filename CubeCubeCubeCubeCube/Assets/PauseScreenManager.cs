using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenManager : MonoBehaviour {
    private Canvas canvas;
    private bool isGamePaused;
    private float previousTimescale;
    private WorldSpaceDisplayController wsdc;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        wsdc = FindObjectOfType<WorldSpaceDisplayController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            Player.instance.playerZaWarudo.enabled = !isGamePaused;
            Player.instance.playerControls.enabled = !isGamePaused;
            Player.instance.playerGun.enabled = !isGamePaused;
            Player.instance.enabled = !isGamePaused;
            CameraRigController.instance.enabled = !isGamePaused;
            GameManager.instance.enabled = !isGamePaused;
            wsdc.enabled = !isGamePaused;
            canvas.enabled = isGamePaused;
            float timescaleToApply = previousTimescale;

            previousTimescale = Time.timeScale;
            Time.timeScale = timescaleToApply;
        }
    }
}
