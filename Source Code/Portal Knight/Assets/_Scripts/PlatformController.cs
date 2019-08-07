using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public float growSpeed = 0.5f;
    public float rotateSpeed = 2f;
    public float deactivateTimer = 5f;
    public bool firstStart = true;
    public bool left = false;
    public bool expand = true;
    public bool rotate = false;
    public float maxSize = 2.25f;
    public Transform parent;
    Vector3 direction;
    bool visible = false;
	// Use this for initialization
	void OnEnable()
    {
        if (parent == null)
        {
            parent = transform.parent;
        }

        if (rotate)
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

            if (left)
            {
                direction = Vector3.back;

            }
            else
            {
                direction = Vector3.forward;
            }
        }
    }

   
    // Update is called once per frame
    void Update ()
    {
        if (!PlayerHealth.dead && Time.timeScale != 0)
        {
            if(expand)transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
            if(rotate && visible)transform.RotateAround(parent.position, direction, rotateSpeed * Time.fixedDeltaTime);
        }

        if (expand && transform.localScale.x >= maxSize)
        {
            Deactivate();
        }

    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void OnBecameInvisible()
    {
        visible = false;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }


}
