using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialControl : MonoBehaviour
{
    public GameObject[] steps;
    int curStep = 0;
    bool called = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            if (i != curStep) steps[i].SetActive(false);
            else steps[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStep)
        {
            case 0: if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !called)
                    {                   
                        Invoke("NextStep", 2f);
                        called = true;
                    }
                break;
            case 1:
                    if (Input.GetKeyDown(KeyCode.W) && !called)
                    {
                        Invoke("NextStep", 2f);
                        called = true;
                    }
                break;
            case 2:
                    if (Input.GetKeyDown(KeyCode.Space) && !called)
                    {
                        Invoke("NextStep", 2f);
                    called = true;
                    }
                break;

        }
    }

    void DeActivate()
    {
        gameObject.SetActive(false);
    }
    
    void NextStep()
    {
        curStep++;
        called = false;
        for (int i = 0; i < 3; i++)
        {
            if (i != curStep) steps[i].SetActive(false);
            else steps[i].SetActive(true);
        }

        if (curStep == 3) DeActivate();
    }
}
