using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] ObjectiveManager manager;
    [SerializeField] List<GameObject> checks;

    void Start()
    {
        for(int i = 0; i < checks.Count; i++)
        {
            checks[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.Objectives.Count != 0)
        {
            for (int i = 0; i < manager.Objectives.Count; i++)
            {
                if (manager.Objectives[i].completed)
                {
                    checks[i].SetActive(true);
                }
            }
        }
       
    }
}
