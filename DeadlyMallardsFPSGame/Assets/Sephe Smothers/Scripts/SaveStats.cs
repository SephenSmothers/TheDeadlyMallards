using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveStatsObj", menuName = "SaveData")]   
public class SaveStats : ScriptableObject
{
    public int _cash = 0;
    public List<GunsManager> _guns = new List<GunsManager>();
    //public List<GunsManager> _guns = GameManager.instance.playerScript.usedGuns;
    //public EnemySpawner _spawnerRef = EnemySpawner.instance;
}
