using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasContoller : MonoBehaviour {

    public Text txtScore;
    public Text txtLvl;
    public GameObject btnUp;
    public GameObject btnDown;
	// Use this for initialization
	void Start ()
    {
        txtScore.text = "Highscore: " + GameSettings.Instance.GetHighScore();
	}

    private void LateUpdate()
    {
        txtLvl.text = "Level " + GameSettings.Instance.m_data.levels[GameSettings.Instance.curLevel].levelNum;
        txtScore.text = "Highscore: " + GameSettings.Instance.GetHighScore();
        if (GameSettings.Instance.curLevel == 0) btnDown.SetActive(false);
        else btnDown.SetActive(true);

        if (GameSettings.Instance.curLevel > GameSettings.Instance.m_data.levels.Count - 1 || !GameSettings.Instance.m_data.levels[GameSettings.Instance.curLevel+1].unlocked) btnUp.SetActive(false);
        else btnUp.SetActive(true);
    }

    public void Play()
    {
        GameSettings.Instance.Play();
    }

    public void Quit()
    {
        GameSettings.Instance.Quit();
    }

    public void NextLevel()
    {
        if (GameSettings.Instance.curLevel < GameSettings.Instance.m_data.levels.Count - 1)
        {
            GameSettings.Instance.curLevel++;
        }
    }

    public void PrevLevel()
    {
        if (GameSettings.Instance.curLevel > 0)
        {
            GameSettings.Instance.curLevel--;
        }

    }
}
