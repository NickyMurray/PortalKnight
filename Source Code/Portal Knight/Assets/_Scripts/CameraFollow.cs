using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target; // Object to follow (Player)
    public float moveSpeed = 0.1f; // Speed the camera will follow at
    public Vector3 distance;
    public Vector3 targetPos;
    // Use this for initialization

    void Awake ()
    {
        if (target == null) target = GameObject.FindGameObjectWithTag("Player");
      
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = target.transform.localPosition;
        distance = targetPos - new Vector3(transform.localPosition.x, transform.localPosition.y);
        if (distance.magnitude >= moveSpeed * Time.deltaTime)
        {
            distance = distance.normalized;
            transform.localPosition += distance.normalized * moveSpeed * Time.deltaTime;

        }

        //Keeps camera perpendicular to current platform
        //Vector3 rotate = target.GetComponent<MovementController>().center.transform.position - transform.position;
        //rotate.z = 0;
        //transform.up = -rotate * Time.deltaTime;
    }

}
