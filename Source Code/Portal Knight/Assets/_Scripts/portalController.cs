using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalController : MonoBehaviour {

    //GameObject canvas;
    GameObject platformCam;
    GameObject canvas;
	// Use this for initialization
	void Start ()
    {
        canvas = GameObject.FindGameObjectWithTag("LevelCanvas");
        if (platformCam == null) platformCam = GameObject.FindGameObjectWithTag("platformCam");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Time.timeScale != 0)
        {
            transform.RotateAround(transform.parent.position, Vector3.back, 200 * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Time.timeScale = 0;
            canvas.GetComponent<LevelCanvas>().Win();
        }
        else if (col.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }
    }
}
