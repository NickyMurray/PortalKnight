using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballController : MonoBehaviour {

    public enum Direction { LEFT, RIGHT };

    public Direction dir = Direction.LEFT;
    Animator anim;
    AudioSource audio;
    public AudioClip burn;
    public AudioClip fire;
    public float speed = 20f;
    public int damage = 10;
    public bool playerBullet = false;
    public GameObject center;
    Vector3 direction;
    bool visible = true;
    bool started = true;

	void Start ()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        started = true;
        Invoke("Deactivate", 10f);
        switch (dir)
        {
            case Direction.LEFT:
                direction = Vector3.forward;
                transform.localScale = new Vector3(-0.1f, 0.1f, 0.1f);
                break;
            case Direction.RIGHT:
                direction = Vector3.back;
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                break;
        }
    }

    private void Update()
    {
        if (visible && started)
        {
            audio.PlayOneShot(fire);
            started = false;
        }
        transform.RotateAround(center.transform.position, direction, speed * Time.deltaTime);
    }
    private void LateUpdate()
    {
        transform.up = (transform.position - center.transform.position);
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    void Deactivate()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            if (visible) audio.PlayOneShot(burn);
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            anim.SetTrigger("explode");
            Invoke("Deactivate", 0.3f);
        }
        else if (col.gameObject.tag == "fireball")
        {
            if (visible) audio.PlayOneShot(burn);
            anim.SetTrigger("explode");
            Invoke("Deactivate", 0.3f);
        }
        else if (col.gameObject.tag == "Enemy")
        {
            if (visible) audio.PlayOneShot(burn);
            anim.SetTrigger("explode");
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Invoke("Deactivate", 0.3f);
        }
        
    }
}
