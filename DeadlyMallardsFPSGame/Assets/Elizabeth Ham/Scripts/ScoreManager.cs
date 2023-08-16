using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public GameObject ScoreBoard;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI zomsKilledText;
    //public TextMeshProUGUI CompleteTimeText;
    public TextMeshProUGUI damageTakenText;
    public TextMeshProUGUI damageDealtText;
   
  
    

    int _Score = 0;
    int damageDealt = 0;
    int damageTaken = 0;
    int zombiesKilled = 0;
    //float completionTime = 0;
    int highScore = 0;


    // Start is called before the first frame update
    private void Start()
    {
     
        instance = this;
        ScoreBoard.SetActive(false);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScores();
        highScoreText.text = "HighScore: " + highScore.ToString();

    }
    public void UpdateScores()
    {
        scoreText.text = "Score: " + _Score.ToString("F0");
        highScoreText.text = "HighScore: " + highScore.ToString();
        zomsKilledText.text = "Zombies Killed: " + zombiesKilled.ToString();
        damageDealtText.text = "Total Damage Dealt: " + damageDealt.ToString();
        damageTakenText.text = "Total Damage Taken: " + damageTaken.ToString();
    }

    //public void ShowScoreBoard(int highscore )
    //{ 
    //    highScoreText.text = "HighScore: " + highscore.ToString();
    //}

    public int AddScore(int _score)
    {
        _Score += _score;
        scoreText.text = "Score: " + _Score.ToString("F0");
        UpdateHighScore(_Score);
        UpdateScores();
        return _Score;

    }

    public void UpdateHighScore(int newHighScore)
    {
        if (newHighScore > highScore)
        {
            highScore = newHighScore;
            highScoreText.text = "HighScore: " + highScore.ToString();
        }
    }

    public void LeaveScoreBoard()
    {
    
        PlayerPrefs.SetInt("HighScore", highScore); 
        PlayerPrefs.Save();
    }

    public void UpdateZombiesKilled(int zombiesKilledIncrement)
    {
        zombiesKilled += zombiesKilledIncrement;
        UpdateScores();
       // return zombiesKilled;
    }

    //public float UpdateCompletionTime(float newCompletionTime)
    //{
    //    completionTime = newCompletionTime;
    //    CompleteTimeText.text = "Completion Time: " + completionTime.ToString("F2") + " seconds";
    //    return completionTime;
    //}

    public int UpdateTotalDamageTaken(int totalDamageTaken)
    {
        damageTaken += totalDamageTaken;
        damageTakenText.text = "Total Damage Taken: " + damageTaken.ToString();
        return damageTaken;
    }

    public void UpdateTotalDamageDealt(int totalDamageDealt)
    {
        damageDealt += totalDamageDealt;
        UpdateScores();
      
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

}
