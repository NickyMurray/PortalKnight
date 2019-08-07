using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour {

    public bool expandingPlatforms = false; //do the platforms grow? if not don't spawn new ones
    public List<GameObject> platforms; //An object pool to hold the level platforms
    public GameObject platform; // The level platform object
    float counter = 0; //A variable to count the interval between spawns
    public float spawnFreq = 3; //How frequent a new object will spawn
    public int platformAmt = 5; // How many platforms will the pool hold
    public float platFormSize = 1;
    private void Start()
    {
        if (expandingPlatforms)
        { 
            //Instantiating the platform objects into the platform pool 
            for (int i = platforms.Count; i < platformAmt; i++)
            {
                    GameObject obj = Instantiate(platform, transform);
                    obj.GetComponent<PlatformController>().maxSize = platFormSize;
                    obj.SetActive(false);
                    platforms.Add(obj);

            }
        }

    }
    void Update()
    {
        if(expandingPlatforms)Spawn();
    }

    void Spawn()
    {
        counter += Time.deltaTime; //Adding the passed time onto the counter
        if (counter >= spawnFreq) //If the counter is greater than the frequency spawn a new object
        {
            foreach (GameObject obj in platforms)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.transform.position = transform.position;
                    obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    obj.SetActive(true);
                    counter = 0;
                    break;
                }
            }
            
        }

    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            col.gameObject.GetComponent<PlatformController>().Deactivate();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        if(GetComponent<CircleCollider2D>())Handles.DrawWireDisc(transform.position, transform.forward, GetComponent<CircleCollider2D>().radius);
    }
#endif
}
