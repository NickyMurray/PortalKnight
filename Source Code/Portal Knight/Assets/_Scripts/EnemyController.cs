using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject center;
    public GameObject player;
    public float speed = 5f;
    Rigidbody2D rb;
    Animator anim;
    EnemyHealth hc;
    SpriteRenderer sr;
    AudioSource audio;
    public AudioClip hurt;
    bool grounded = false;
    int movement = 0;
    public int damage = 20;
    public bool dead = false;
	// Use this for initialization
	void Start ()
    {
        hc = GetComponent<EnemyHealth>();
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        sr.color = GameSettings.Instance.enemyColor;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
        if (hc.dead)
        {          
            anim.SetBool("dead",true);
            Invoke("DetachAndDestroy", 0.5f);
        }

	}

    private void LateUpdate()
    {
        if(GetComponent<SpriteRenderer>().color != GameSettings.Instance.enemyColor)
        {
            GetComponent<SpriteRenderer>().color = GameSettings.Instance.enemyColor;
        }
    }
    private void FixedUpdate()
    {
        if (!hc.dead)
        {
            transform.RotateAround(center.transform.position, Vector3.forward, movement * Time.deltaTime * speed);

            if (!grounded)
            {
                rb.AddForce(new Vector2(transform.position.x - center.transform.position.x, transform.position.y - center.transform.position.y) * Time.deltaTime * -GameSettings.Instance.gravity);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            transform.up = (transform.position - center.transform.position);
        }
    }
    void DetachAndDestroy()
    {
        //transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            audio.PlayOneShot(hurt);
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        else if (col.gameObject.tag == "fireball")
        {
            audio.PlayOneShot(hurt);
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Platform")
        {
            grounded = true;
            // audio.PlayOneShot(hurt);
            if (col.gameObject.GetComponent<PlatformController>().left)
            {
                movement = 1;
            }
            else
            {
                movement = -1;
            }
        }
        

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Platform")
        {
            grounded = false;
        }
    }
}
