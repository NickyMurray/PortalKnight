using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static int health = 100;
    public Slider sldHealth;
    public Text txtScore;
    public Text txtTime;
    public GameObject canvas;
    public Animator damageSreen;
    public static bool dead = false;
    public static bool revive = false;
    public static bool win = false;
    public static float levelTimer = 0;
    float counter = 0;
    public float healthTimer = 0;
    bool damaged = false;
    Animator anim;
    AudioSource audio;
    public AudioClip hurt;
   

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        canvas = GameObject.FindGameObjectWithTag("LevelCanvas");
        dead = false;
        win = false;
    }
    // Update is called once per frame
    void Update ()
    {
        if (dead == true && !revive)
        {
            canvas.GetComponent<LevelCanvas>().Death();
        }
        else
        {
            if (win == false)
            {
                levelTimer += Time.deltaTime;
                if (damaged && counter <= healthTimer)
                {
                    counter += Time.deltaTime;
                }
                else if (counter > healthTimer)
                {
                    damaged = false;
                    if(health < 100)health += 1;
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                   
                    canvas.GetComponent<LevelCanvas>().Pause();
                }

            }
        }
	}

    public static void Heal(int amt)
    {
        health += amt;
        if (health > 100) health = 100;
    }

    private void LateUpdate()
    {
        sldHealth.value = health;
        txtTime.text = "Time: " + levelTimer.ToString("0.00");
        txtScore.text = "Score: " + GameSettings.Instance.curScore.ToString();
        if (revive)
        {
            dead = false;
            health = 100;
            revive = false;
        }
    }

    public void TakeDamage(int amt)
    {
        if (Time.timeScale != 0)
        {
            audio.clip = hurt;
            damageSreen.SetTrigger("hurt");
            audio.loop = false;
            audio.Play();
            if (!dead)
            {
                health -= amt;
                damaged = true;
                counter = 0;
                anim.SetTrigger("Hurt");
                if (health <= 0)
                {
                    dead = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Thorns")
        {
            TakeDamage(25);
        }

    }

    
}
