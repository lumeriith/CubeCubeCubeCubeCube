using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverScreenManager : MonoBehaviour {
    public Image backdrop;
    public GameObject everythingElse;
    public Text scoreText;
    public Text highscoreText;
    public GameObject newHighscoreGroup;
    public GameObject clickToRestart;
    public Text contents;
    private void Start()
    {

    }


    private void OnEnable()
    {
        StopCoroutine("ShowRoutine");
        StartCoroutine("ShowRoutine");
    }

    IEnumerator ShowRoutine()
    {
        yield return new WaitForSecondsRealtime(.1f);
        Color originalBackdropColour = backdrop.color;
        Color colour = backdrop.color;

        for(float t = 0; t < 1.5f; t += Time.unscaledDeltaTime)
        {
            colour.a = t/1.5f * originalBackdropColour.a;
            backdrop.color = colour;
            yield return null;
        }
        backdrop.color = originalBackdropColour;
        yield return new WaitForSecondsRealtime(.75f);
        everythingElse.SetActive(true);
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        if(highscore < (int)GameManager.instance.score)
        {
            StartCoroutine(HighscoreBlinkRoutine());
            PlayerPrefs.SetInt("Highscore", (int)GameManager.instance.score);
        }
        highscoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString("#,#");
        scoreText.text = ((int)GameManager.instance.score).ToString("#,#");
        string travelDistanceString;
        string playTimeString;

        travelDistanceString = ((int)(GameManager.instance.travelDistance)).ToString("#,#0") + "m";


        

        playTimeString = ((int)(GameManager.instance.playTime / 60)).ToString("0") + ":" + (GameManager.instance.playTime - ((int)(GameManager.instance.playTime / 60)) * 60).ToString("00");
        contents.text = travelDistanceString + "\n" + playTimeString + "\n" + GameManager.instance.killCount.ToString("#,#")+ "\n" + (GameManager.instance.difficulty == 0 ? "-" : new string('★',GameManager.instance.difficulty));
        yield return new WaitForSecondsRealtime(1.8f);
        clickToRestart.SetActive(true);



    }

    IEnumerator HighscoreBlinkRoutine()
    {
        while (true)
        {
            newHighscoreGroup.SetActive(!newHighscoreGroup.activeSelf);
            yield return new WaitForSecondsRealtime(.75f);
        }
    }
}
