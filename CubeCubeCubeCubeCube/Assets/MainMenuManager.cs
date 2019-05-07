using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour {
    public Texture2D cursorTexture;
    public Text highscoreText;

    private Vector3 originalClickToStartPosition;
    private void Start()
    {
        highscoreText.text = "High Score " + PlayerPrefs.GetInt("Highscore", 0).ToString("#,#0");


    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, new Vector3(30, 30), CursorMode.Auto);
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("Main");
        }


    }
}
