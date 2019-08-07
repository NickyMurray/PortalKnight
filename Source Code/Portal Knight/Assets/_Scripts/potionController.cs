using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potionController : MonoBehaviour
{
    public int HealthValue = 50;
    AudioSource aS;
    // Update is called once per frame
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "fireball" && col.gameObject.GetComponent<fireballController>().playerBullet)
        {
            aS.Play();
            PlayerHealth.Heal(HealthValue);
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 1);
        }
    }
}
