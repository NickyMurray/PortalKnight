using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerController : MonoBehaviour
{
    public enum EnemyState { Idle, Chase, Attack};

    public EnemyState state = EnemyState.Idle;

    public GameObject player;
    public GameObject center;
    GameObject playerCen;
    EnemyHealth hc;
    Rigidbody2D rb;
    public Vector3 direction;
    Vector3 gravityDirection;
    Vector2 playerPos;
    public AudioClip attackClip;
    public AudioClip hurtClip;
    AudioSource aS;
    public float speed = 5f;
    public float timeBetweenAttacks = 4f;
    public int damage = 5;
    [Header("If the enemy shoots fill these variables")]
    public bool shoots = false;
    public GameObject projectile;
    public GameObject bulletSpawn;
    float counter = 0;
    List<GameObject> bullets = new List<GameObject>();
    public int bulletAmt = 10;
    [Header("If the enemy is a bloodsucker fill these variables")]
    public bool bloodSucker = false;
    public SpriteRenderer body;
    public Color bodyCol;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        hc = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();
        if (body != null) bodyCol = body.color;
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        playerCen = player.GetComponentInChildren<MovementController>().center;

        if (playerCen == center)
        {
            state = EnemyState.Chase;
        }
        else
        {
            state = EnemyState.Idle;
        }

        if (shoots)
        {
            for (int i = 0; i < bulletAmt; i++)
            {
                GameObject bullet;
                bullet = Instantiate(projectile, transform.position, transform.rotation);
                bullets.Add(bullet);
                bullet.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gravityDirection = transform.position - center.transform.position;
        gravityDirection = gravityDirection.normalized;
        playerCen = player.GetComponentInChildren<MovementController>().center;

        if (playerCen == center)
        {
            if (state == EnemyState.Idle) state = EnemyState.Chase;
        }
        else
        {
            state = EnemyState.Idle;
        }

        if (!hc.dead)
        {
            switch (state)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Chase:
                    MoveToPlayer();
                    if (shoots) Attack();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;

            }

            if (counter < timeBetweenAttacks)
            {
                counter += Time.deltaTime;
            }
 
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }


    }

    private void LateUpdate()
    {
        if (bloodSucker) body.color = bodyCol;
    }

    private void FixedUpdate()
    {
        if(hc.dead)
        {
            rb.AddForce(gravityDirection * Time.deltaTime * -500);
        }
    }

    void Idle()
    {
        aS.Stop();
        transform.up = gravityDirection;
    }

    void MoveToPlayer()
    {
        if (aS.isPlaying == false) aS.Play();
        playerPos = player.GetComponentInChildren<Rigidbody2D>().position;
        direction = new Vector3(playerPos.x, playerPos.y, 0) - transform.position;

        if (direction.normalized.x > 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        else transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);

        if (direction.magnitude > 3 * (Time.deltaTime * speed))
        {
            direction = direction.normalized;

            transform.position += direction * Time.deltaTime * speed;
        }

        transform.up = gravityDirection;
        
    }

    void Attack()
    {
        if (counter > timeBetweenAttacks)
        {
            //if the enemy shoots bullets spawn bullet, else melee
            if (shoots)
            {
                foreach (GameObject b in bullets)
                {
                    if (!b.activeInHierarchy)
                    {
                        b.transform.position = bulletSpawn.transform.position;

                        if (transform.localScale.x > 0)
                        {
                            b.GetComponent<fireballController>().dir = fireballController.Direction.LEFT;
                            b.transform.localScale = new Vector3(-Mathf.Abs(b.transform.localScale.x), b.transform.localScale.y, b.transform.localScale.z);
                        }
                        else
                        {
                            b.GetComponent<fireballController>().dir = fireballController.Direction.RIGHT;
                        }
                        b.GetComponent<fireballController>().center = center;
                        b.SetActive(true);
                        break;
                    }
                }
               
            }
            else if (bloodSucker)
            {
                player.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);
                bodyCol = new Color(1, body.color.g - 0.1f, body.color.b - 0.1f);

                if (body.transform.localScale.x < 1.6f)
                {
                    body.transform.localPosition += new Vector3(0.05f, -0.045f, 0f);
                    body.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
            else
            {
                player.GetComponentInChildren<PlayerHealth>().TakeDamage(damage);

            }

            if (attackClip != null)
            {
                aS.PlayOneShot(attackClip);
            }
            counter = 0;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            state = EnemyState.Attack;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            state = EnemyState.Chase;
        }
    }


}
