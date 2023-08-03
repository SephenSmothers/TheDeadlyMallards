using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour, Interactables
{

    [SerializeField] List<Collectible> Objectives;
    [SerializeField] int roundToReach;
    public int zombiesToKill;
    public bool AllObjectivesCompleted;

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

            if (Objectives[i].completed && zombiesToKill <= GameManager.instance.GetZombiesKilled() && roundToReach <= EnemySpawner.instance.GetCurrentWave())
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

    public void Interact()
    {
        CheckObjectivesComplete();
    }

    public string promptUi()
    {
        return "Press 'E' To Continue";
    }
}
