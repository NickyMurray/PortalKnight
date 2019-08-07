using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinController : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject center;
    AudioSource aS;
    public AudioClip sound;
    Vector3 gravityDirection;
    Collider2D col;
    float timer = 0.1f;
    float counter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        aS = GetComponent<AudioSource>();

        Destroy(gameObject, 30f);
    }

    private void Update()
    {
        if (counter < timer)
        {
            counter += Time.deltaTime;
        }
        else
        {
            if(col.isTrigger) col.isTrigger = false;
        }
    }

    private void FixedUpdate()
    {
        if(counter < timer) rb.AddForce(gravityDirection * Time.deltaTime * -500);
        else rb.AddForce(gravityDirection * Time.deltaTime * 50);
    }

    public void setCenter(GameObject cen)
    {
        center = cen;
        gravityDirection = center.transform.position - transform.position;
        gravityDirection = gravityDirection.normalized;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && counter >= timer)
        {
            aS.PlayOneShot(sound);
            GameSettings.Instance.AddCoins(5);
            GameSettings.Instance.AddScore(5);
            Destroy(gameObject, 0.3f);
        }
    }
}
