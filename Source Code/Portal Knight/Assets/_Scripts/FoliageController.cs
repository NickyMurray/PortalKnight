using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    public List<Sprite> Plants;
    public SpriteRenderer sr;
    public GameObject coin;
    AudioSource aS;
    public AudioClip clip;
    bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        aS.volume = 0.05f;
        if (sr == null) sr.GetComponent<SpriteRenderer>();
        sr.sprite = Plants[Random.Range(0, Plants.Count)];

        coin = GameSettings.Instance.coin;
    }

    private void OnEnable()
    {
        if (sr == null) sr.GetComponent<SpriteRenderer>();
        sr.sprite = Plants[Random.Range(0, Plants.Count)];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("shake");
            aS.PlayOneShot(clip);
            if (!triggered)
            {
                int rand = Random.Range(0, 3);
                for (int i = 1; i <= rand; i++)
                {
                    GameObject obj;
                    obj = Instantiate(coin, transform.position + new Vector3(0, -0.2f, 0), Quaternion.Euler(Vector3.zero));
                    obj.GetComponent<coinController>().setCenter(col.GetComponent<MovementController>().center);
                }

                triggered = true;
            }
        }
    }

}
