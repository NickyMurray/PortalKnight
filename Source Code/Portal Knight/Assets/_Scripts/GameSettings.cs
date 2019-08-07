using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameSettings : MonoBehaviour
{

    public static GameSettings Instance { get; private set; }

    //General Game variables
    public float highScore = 0;
    public int curScore = 0;
    public int curLevel = 0;
    public int spawnAmount = 1;
    public int maxAmount = 15;
    public int coins = 0;
    public int lives = 3;
    public Color enemyColor;
    public bool playAgain = false;
    public float gravity = 10f;
    public GameObject coin;
    public GameObject curPlatform;
    public GameData m_data = new GameData();

    void Awake()
    {
        //Creating a new Instance of the GameSettings Object
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            enemyColor = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0.0f, 1f);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        Load();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main_Menu")
        {
            PlayerHealth.dead = false;
            PlayerHealth.levelTimer = 0;
            Time.timeScale = 1;
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.PK");

        //Create new data object and assign new data
        GameData data = m_data;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("saved");
    }

    public void Load()
    {
        Debug.Log(Application.persistentDataPath + "/gameData.PK");
        if (File.Exists(Application.persistentDataPath + "/gameData.PK"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.PK", FileMode.Open);

            //Assign loaded data to the current gamedata
            m_data = (GameData)bf.Deserialize(file);

            Debug.Log("loaded");
        }
        else
        {
            // if no save data found create new variables
            m_data = new GameData();
            m_data.levels = new List<LevelData>();
            for(int i = 0; i < 10; i++)
            {
                LevelData ld = new LevelData();
                ld.levelNum = i + 1;

                if (i == 0)
                {          
                    ld.unlocked = true;                 
                }

                ld.levelNum = i + 1;
                m_data.levels.Add(ld);
            }
            Debug.Log("new load created");
        }
    }

    public void NextLevel()
    {
        curLevel++;
        curScore = 0;
        SetHighScore();
        Save();
        if (spawnAmount < 5) spawnAmount++;
        PlayerHealth.levelTimer = 0;
        Instance.GetComponent<SpawnController>().maxAmount += 1;
        enemyColor = UnityEngine.Random.ColorHSV(0f, 1f, 0f, 1f, 0.0f, 1f);
        Time.timeScale = 1;

        SceneManager.LoadScene("level" + curLevel);
    }

    public int GetHighScore()
    {
        return m_data.levels[curLevel].score;
    }

    public void UnlockNextLevel()
    {
        m_data.levels[curLevel + 1].unlocked = true;
    }

    public void Play()
    {
        SceneManager.LoadScene("level" + curLevel);
    }

    public void GoToMenu()
    {
        SetHighScore();
        Save();
        SceneManager.LoadScene("Main_Menu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PlayerHealth.revive = true;
        lives = 3;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        SetHighScore();
        Save();
        Application.Quit();
    }

    public bool SetHighScore()
    {
        if (curScore > m_data.levels[curLevel].score)
        {
            m_data.levels[curLevel].score = curScore;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddScore(int amt)
    {
        curScore += amt;
    }

    public void AddCoins(int amt)
    {
        coins += amt;

        if (coins >= 100 && lives < 3)
        {
            lives++;
            coins -= 100;
        }
    }

    public void ResetScore()
    {
        curScore = 0;
        coins = 0;
    }
}

[Serializable]
public class LevelData
{
    public int levelNum;
    public bool unlocked = false;
    public int score;
}

[Serializable]
public class GameData
{
    public List<LevelData> levels;
}
