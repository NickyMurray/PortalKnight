using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> enemies;
    [SerializeField]
    List<GameObject> spawns;
    bool firstStart = true;
    int amtToSpawn = 0;
    // Use this for initialization
    void OnEnable()
    {
        if (!firstStart)
        {
            Spawn();   
        }
    }

    private void OnDisable()
    {
        foreach(GameObject obj in spawns)
        {
            Destroy(obj);
        }
    }

    private void Start()
    {
        if (firstStart)
        {
            firstStart = false;
        }

    }

    public void Spawn()
    {
        int randEnemy = Random.Range(0, enemies.Count - 1);
        amtToSpawn = Random.Range(0, GameSettings.Instance.spawnAmount + 1);
        float randChance = Random.Range(0, 10);
        if (randChance > 5)
        {
            for (int i = 0; i <= amtToSpawn; i++)
            {
                GameObject spawn;
                spawn = Instantiate(enemies[randEnemy], transform.position, Quaternion.Euler(Vector3.zero));
                spawn.GetComponent<EnemyController>().center = transform.parent.transform.parent.gameObject;
                spawn.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                spawns.Add(spawn);
            }
        }
    }

}
