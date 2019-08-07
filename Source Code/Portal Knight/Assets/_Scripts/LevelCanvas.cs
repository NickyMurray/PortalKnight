using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour {

    public GameObject deathScreen;
    public GameObject btnPlayOn;
    public GameObject btnRestart;
    public GameObject winScreen;
    public GameObject pauseScreen;
    public Text txtNextLevel;
    public Text txtCoin;
    public Text txtWinScore;
    public Text txtDeathScore;
    public GameObject[] hearts = new GameObject[3];
    public bool pause;

	// Use this for initialization
	void Start ()
    {
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
        pauseScreen.SetActive(false);
	}

    private void LateUpdate()
    {
        txtCoin.text = "x " + GameSettings.Instance.coins;

        switch (GameSettings.Instance.lives)
        {
            case 1: hearts[0].SetActive(true);
                    hearts[1].SetActive(false);
                    hearts[2].SetActive(false);
                break;
            case 2: hearts[0].SetActive(true);
                    hearts[1].SetActive(true);
                    hearts[2].SetActive(false);
                break;
            case 3: hearts[0].SetActive(true);
                    hearts[1].SetActive(true);
                    hearts[2].SetActive(true);
                break;
        }
    }

    public void Death()
    {
        Time.timeScale = 0;
        txtDeathScore.text = "Highscore: " + GameSettings.Instance.highScore;
        //GetComponent<AudioSource>().Stop();
        deathScreen.SetActive(true); 
        if(GameSettings.Instance.lives > 1)
        {
            btnPlayOn.SetActive(true);
            btnRestart.SetActive(false);
        }
        else
        {
            btnRestart.SetActive(true);
            btnPlayOn.SetActive(false);
        }
    }

    public void Win()
    {
        GameSettings.Instance.SetHighScore();
        txtWinScore.text = "Highscore: " + GameSettings.Instance.GetHighScore();
        GameSettings.Instance.UnlockNextLevel();
        PlayerHealth.win = true;
        GetComponent<AudioSource>().Stop();
        winScreen.SetActive(true);
        if(GameSettings.Instance.curLevel >= GameSettings.Instance.m_data.levels.Count - 1) txtNextLevel.text = "Back to Menu";
        GameSettings.Instance.Save();
    }

    public void Pause()
    {
        if (pause == false)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            pause = true;
        }
        else
        {
            pause = false;
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
    }
    public void PlayAgain()
    {
        Time.timeScale = 1;
        GameSettings.Instance.lives--;
        GameSettings.Instance.playAgain = true;
        GameSettings.Instance.RestartLevel();
    }

    public void PlayOn()
    {
        Time.timeScale = 1;
        GameSettings.Instance.lives--;
        PlayerHealth.revive = true;
        deathScreen.SetActive(false);
    }

    public void NextLevel()
    {
        if (GameSettings.Instance.curLevel >= GameSettings.Instance.m_data.levels.Count - 1)
        {
            GameSettings.Instance.GoToMenu();
        }
        else GameSettings.Instance.NextLevel();
    }

    public void Quit()
    {
        GameSettings.Instance.GoToMenu();
    }
}
