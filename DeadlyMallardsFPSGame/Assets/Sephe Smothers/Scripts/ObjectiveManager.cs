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
    public bool LastObjective;
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
            if (LastObjective)
            {
                AllObjectivesCompleted = true;
                GameManager.instance.SaveAllStats();

                LevelWinUi.SetActive(true);
                ObjectiveUi.SetActive(false);
                _scoreManager.ScoreBoard.SetActive(true);
                StartCoroutine(HideLevelCompleteUI());
            }
            else if (CheckObjectives())
            {
                AllObjectivesCompleted = true;
                GameManager.instance.SaveAllStats();

                LevelWinUi.SetActive(true);
                ObjectiveUi.SetActive(false);
                _scoreManager.ScoreBoard.SetActive(true);
                StartCoroutine(HideLevelCompleteUI());
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

    private IEnumerator HideLevelCompleteUI()
    {
      

        float timerDuration = 3.0f;
        float startTime = Time.realtimeSinceStartup;



        GameManager.instance.Pause();


        while (Time.realtimeSinceStartup - startTime < timerDuration)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();


        

       SceneManager.LoadSceneAsync(nextScene);


        
        _scoreManager.ScoreBoard.SetActive(false);
        LevelWinUi.SetActive(false);
        ObjectiveUi.SetActive(true);
        
        GameManager.instance.UnPause();

        

        //yield return new WaitForSeconds(3.0f);
        //Debug.Log("Disabling player collider");
        //playerCollider.enabled = false;
        //yield return new WaitForSeconds(3.0f);
        //Debug.Log("Enabling player collider");
        //playerCollider.enabled = true;

        //_scoreManager.ScoreBoard.SetActive(false);
        //LevelWinUi.SetActive(false);
        //ObjectiveUi.SetActive(true);
        //playerCollider.enabled = true;


       
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
