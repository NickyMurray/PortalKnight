using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            col.gameObject.GetComponent<PlatformController>().Deactivate();
        }
        if (col.gameObject.tag == "fireball")
        {
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyController>().dead = true;
        }
    }
}
