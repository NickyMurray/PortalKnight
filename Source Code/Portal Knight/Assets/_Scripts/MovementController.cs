using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


    public float speed = 500f;
    //Control variables
    public float movement = 0;
    public float gravity = 20f;
    public float jumpForce = 500f;
    public bool walk = false;
    public bool attack = false;
    public bool grounded = false;
    public bool jump = false;
    GameObject lastPlat;
    bool facingLeft = false;
    //public int quadrant = 0;
    public float rotZ = 0;
    public float jumpTimer = 1f;
    float counter = 0;
    public GameObject center;
    public GameObject parent;
    public GameObject projectile;
    public GameObject bulletSpawn;
    List<GameObject> bullets = new List<GameObject>();
    public int bulletAmt = 30;

    public Vector3 gravityDirection;

    Rigidbody2D rb;
    Animator anim;
    AudioSource audio;
    public AudioClip acWalk;
	// Use this for initialization
	void Start ()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        GameSettings.Instance.curPlatform = center;

        for (int i = 0; i < bulletAmt; i++)
        {
            GameObject bullet;
            bullet = Instantiate(projectile, transform.position, transform.rotation);
            bullets.Add(bullet);
            bullet.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (!PlayerHealth.dead)
        {
            movement = Input.GetAxisRaw("Horizontal");

            gravityDirection = transform.position - center.transform.position;
            gravityDirection = gravityDirection.normalized;
            //If the player presses space bar shoot
            if (Input.GetKeyDown(KeyCode.Space)) attack = true;
            //if the player presses W, jump
            if(Input.GetKeyDown(KeyCode.W) && !jump) jump = true;
            if (jump && counter < jumpTimer && grounded)
            {
                counter += Time.deltaTime;
            }
            else
            {
                jump = false;
                counter = 0;
            }


            if (movement != 0) walk = true;
            else walk = false;

            if (movement < 0) facingLeft = true;
            else if (movement > 0) facingLeft = false;

            Animate();
            //Call shoot method if shoot is true
            if (attack) Shoot();

            Sound();
        }
    }


    private void FixedUpdate()
    {
        if (!PlayerHealth.dead)
        {
            parent.transform.RotateAround(center.transform.position, Vector3.forward, movement * Time.deltaTime * -speed);

            if(jump)
            {
                rb.AddForce(gravityDirection * Time.deltaTime * jumpForce);
                jump = false;
            }
      
            if(!grounded) rb.AddForce(gravityDirection * Time.deltaTime * -gravity);
            else if(grounded && !jump)
            {
                rb.velocity = Vector3.zero;
            }

            transform.up = (transform.position - center.transform.position);

        }
    }

    private void LateUpdate()
    {
        if (lastPlat != null)
        {
            if (lastPlat.activeInHierarchy == false && grounded) grounded = false;
        }
    }

    void Sound()
    {
        if (walk  && grounded && audio.isPlaying == false)
        {
            audio.loop = true;
            audio.clip = acWalk;
            audio.Play();
        }
        else if ((!walk || !grounded) && audio.isPlaying)
        {
            audio.Stop();
        }
    }

    void Shoot()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.transform.position = bulletSpawn.transform.position;
                
                if (facingLeft)
                {
                    bullet.GetComponent<fireballController>().dir = fireballController.Direction.LEFT;                    
                }
                else
                {
                    bullet.GetComponent<fireballController>().dir = fireballController.Direction.RIGHT;
                }

                bullet.GetComponent<fireballController>().center = center;
                bullet.SetActive(true);
                attack = false;
                break;
            }
        }
        
    }

    void Animate()
    {
        anim.SetBool("Walk", walk);
        anim.SetBool("Grounded", grounded);
        if(attack)anim.SetTrigger("Attack");
        if (PlayerHealth.dead) anim.SetBool("Dead",true);

        if (facingLeft)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            grounded = true;
            lastPlat = col.gameObject;
        } 
    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == "Platform")
        {
            grounded = false;
        }
    }

}
