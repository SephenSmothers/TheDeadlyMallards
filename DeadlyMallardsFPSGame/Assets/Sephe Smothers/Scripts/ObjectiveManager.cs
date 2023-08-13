using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveManager : MonoBehaviour, Interactables
{
    int nextScene;
    public List<Collectible> Objectives;
    //[SerializeField] int roundToReach;
   // public int zombiesToKill;
    public bool AllObjectivesCompleted;
    [SerializeField] bool LastObjective;


    // Start is called before the first frame update
    void Start()
    {
        AllObjectivesCompleted = false;
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;

    }

    // Update is called once per frame
    //void OnDisable()
    //{
    //    GameManager.instance.LoadAllStats();
    //}

    public void CheckObjectivesComplete()
    { 
        for (int i = 0; i < Objectives.Count; i++)
        {

            if (CheckObjectives())
            {
                //if (LastObjective)
                //{
                //    AllObjectivesCompleted = true;
                //    GameManager.instance.ResetAllStats();
                //    SceneManager.LoadScene(nextScene);
                //}
                //else
                //{
                AllObjectivesCompleted = true;
                GameManager.instance.SaveAllStats();
                SceneManager.LoadScene(nextScene);
                // } 
            }
            else
            {
                AllObjectivesCompleted = false;
                return;
            }
        }
    }

    private bool CheckObjectives()
    {
        bool complete = false;

        if (Objectives.Count == 0 || Objectives == null)
        {
            complete = true;
        }
        else
        {
            for (int i = 0; i < Objectives.Count; i++)
            {
                if (Objectives[i].completed)
                {
                    complete = true;
                }
                else
                {
                    complete = false;
                    break;
                }
            }
        }

        return complete;
    }
    //private bool CheckZombies()
    //{
    //    bool complete = false;

    //    if (zombiesToKill <= GameManager.instance.GetZombiesKilled())
    //    {
    //        complete = true;
    //    }

    //    return complete;
    //}
    //private bool CheckWaves()
    //{
    //    bool complete = false;

    //    if (roundToReach <= EnemySpawner.instance.GetCurrentWave())
    //    {
    //        complete = true;
    //    }

    //    return complete;
    //}

    public void Interact()
    {
        CheckObjectivesComplete();
    }

    public string promptUi()
    {
        return "Press 'E' To Continue";
    }
}
