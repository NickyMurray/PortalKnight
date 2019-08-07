using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public GameObject target;
    public GameObject startPos;
    public GameObject platformCam;
    public Vector2 centreDiff;
    public Vector3 targetDiff;
    public float actualDistanceFromCentre;
    public float speed = 5f;
    public bool newPos = true;
    // Start is called before the first frame update
    private void Awake()
    {
        if (platformCam == null) platformCam = GameObject.FindGameObjectWithTag("platformCam");
        platformCam.SetActive(false);
    }

    void Start()
    {
        targetDiff = target.transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * 0) * 1.525f, Mathf.Cos(Mathf.Deg2Rad * 0) * 1.525f, -10);
        
    }


    public void SetStart()
    {
        transform.position = startPos.transform.position + new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        targetDiff = target.transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * 0) * 2.25f, Mathf.Cos(Mathf.Deg2Rad * 0) * 2.25f, -10);
        centreDiff = targetDiff - new Vector3(transform.position.x, transform.position.y);
        actualDistanceFromCentre = centreDiff.magnitude;
        if (actualDistanceFromCentre > Time.unscaledDeltaTime * speed && newPos)
        {
            if (Time.timeScale != 0) Time.timeScale = 0;
            centreDiff = centreDiff.normalized;
            transform.Translate(centreDiff.normalized * Time.unscaledDeltaTime * speed);
        }
        else
        {
            
            Time.timeScale = 1;
            
            platformCam.SetActive(true);
            platformCam.transform.position = target.transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * 0) * 2.6f, Mathf.Cos(Mathf.Deg2Rad * 0) * 2.6f, -10);
            gameObject.SetActive(false);
        }
    }
}
