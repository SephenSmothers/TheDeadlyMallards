using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField] List<List<Collectible>> Objectives; 
    bool AllObjectivesCompleted;

    // Start is called before the first frame update
    void Start()
    {
        AllObjectivesCompleted = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckObjectivesComplete() 
    {
        for (int i = 0; i < Objectives.Count; i++)
        {
            for (int j = 0; j < Objectives[i].Count; j++)
            {
                if (Objectives[i][j].completed)
                {
                    AllObjectivesCompleted = true;
                }
                else
                {
                    AllObjectivesCompleted = false;
                    return; 
                }
            }
        }
    }
}
