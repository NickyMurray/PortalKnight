using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSwapper : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameObject playerCenter = col.GetComponentInChildren<MovementController>().center;
            if (playerCenter != gameObject)
            {
                col.GetComponentInChildren<MovementController>().center = gameObject;
                GameSettings.Instance.curPlatform = gameObject;
            }
        }
        else if (col.tag == "Enemy")
        {
            GameObject objCenter;

            if (col.GetComponent<FlyerController>() != null)
            {
                objCenter = col.GetComponentInChildren<FlyerController>().center;
                if (objCenter != gameObject)
                {
                    col.GetComponentInChildren<FlyerController>().center = gameObject;
                }
            }
            else if (col.GetComponent<EnemyController>() != null)
            {
                objCenter = col.GetComponentInChildren<EnemyController>().center;
                if (objCenter != gameObject)
                {
                    col.GetComponentInChildren<EnemyController>().center = gameObject;
                }
            }
        }
    }
}
