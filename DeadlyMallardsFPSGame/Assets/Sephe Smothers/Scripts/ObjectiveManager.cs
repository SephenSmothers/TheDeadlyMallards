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
    public GameObject LevelWinUi;
    public GameObject ObjectiveUi;
    public ScoreManager _scoreManager;


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

                LevelWinUi.SetActive(true);
                ObjectiveUi.SetActive(false);
                _scoreManager.ScoreBoard.SetActive(true);
                StartCoroutine(HideLevelCompleteUI());
                //SceneManager.LoadScene(nextScene);
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

    private IEnumerator HideLevelCompleteUI()
    {
        GameManager.instance.Pause();
        yield return new WaitForSeconds(3.0f);

        _scoreManager.ScoreBoard.SetActive(false);
        LevelWinUi.SetActive(false);
        ObjectiveUi.SetActive(true);

       
        SceneManager.LoadScene(nextScene);
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
