using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    AudioSource audio;
    public AudioClip hurt;
    Animator anim;
    public int value = 20;
    public int health = 40;
    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int amt)
    {
        health -= amt;
        anim.SetTrigger("hurt");
        //audio.PlayOneShot(hurt);
        if (health <= 0 && dead == false)
        {
            GameSettings.Instance.curScore += value;
            dead = true;
        }
    }


}
