using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RoundData
{
    public int CurrentRound;
    public int MonstersSpawned;
    public string RoundDuration;
}

[System.Serializable]
public class RoundDataList
{
    public List<RoundData> roundsData = new List<RoundData>();
}

public class RoundManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    MonsterObjectPool _monsterPool = null;

    [SerializeField]
    GuiManager _guiManager = null;

    

    [Header("Round Settings")]
    [SerializeField]
    float _delayBeforeNextRound = 2f;
    
    float _InitialDelayBeforeRound = 0.0f;

    [SerializeField]
    float _roundDuration = 0.0f;
    
    int _currentRound = 0;
    int _previousRoundValue = 0;
    int _currentRoundValue = 1;


    [Header("Data")]
    bool _startCounter = false;
    RoundDataList roundDataList = new RoundDataList();
    string _savePath = "RoundDatas.json";

    bool _hasRealesedMonster = false;

    

    public void Initialize()
    {
        _InitialDelayBeforeRound = _delayBeforeNextRound;
        _guiManager.UpdateRound(_currentRound);
    }

    public void UpdateExternal(MonsterManager monsterManager)
    {
        if(monsterManager.CheckMonsterInScreen())
        {
            _startCounter = false;
            if(_hasRealesedMonster)
            {
                SaveRoundData();
                _hasRealesedMonster = false;
            }
        }

        if(_startCounter)
        {
            _roundDuration += Time.deltaTime;
            _guiManager.UpdateRoundDuration(_roundDuration);
        }
        else
        {
            _roundDuration = 0;
        }
    }
    public void StartNextRound(MonsterManager monsterManager, Transform spawnPoint)
    {
        if(_currentRound == 0)
        {
            InitializeFibonacciSequence();
        }
        else
        {
            CalculateNextFibonacciValue();
        }
        // Start spawning monsters for the current round
        _hasRealesedMonster =  true;
        SpawnMonstersRecursively(_currentRoundValue, monsterManager, spawnPoint);
        _currentRound++;
        _guiManager.UpdateRound(_currentRound);
    }

    public void DelayBeforeSpawn(MonsterManager monsterManager, Transform spawnPoint)
    {
        if(_delayBeforeNextRound <= 0)
        {
            _startCounter = true;
            StartNextRound(monsterManager, spawnPoint);
            
            _delayBeforeNextRound = _InitialDelayBeforeRound;
        }
        else
        {
            _delayBeforeNextRound -= Time.deltaTime;
        }
    }

    void InitializeFibonacciSequence()
    {
        _previousRoundValue = 0;
        _currentRoundValue = 1;
    }

    public void CalculateNextFibonacciValue()
    {
        int nextRoundValue = _previousRoundValue + _currentRoundValue;
        _previousRoundValue = _currentRoundValue;
        _currentRoundValue = nextRoundValue;
    }

    public void SpawnMonstersRecursively(int remainingCount, MonsterManager monsterManager, Transform spawnPoint)
    {
        if (remainingCount > 0)
        {
            // Spawn a monster
            GameObject monster = _monsterPool.GetMonsterFromPool();
            Monster monsterPrefab = monster.GetComponent<Monster>();
            monsterPrefab.gameObject.transform.position = spawnPoint.position;

            // Initialize the monster and subscribe to the OnDestroy event
            monsterPrefab.Initialize();
            monsterPrefab.gameObject.SetActive(true);

            // Notify MonsterManager that a new monster is active
            monsterManager.AddMonster(monsterPrefab);

            // Recursively call to spawn the next monster
            SpawnMonstersRecursively(remainingCount - 1, monsterManager, spawnPoint);
        }
    }

    public void SaveRoundData()
    {
        // Ensure that _saveFileName is defined somewhere in your class
        string saveFilePath = Path.Combine(Application.persistentDataPath, _savePath);
        int minutes = Mathf.FloorToInt(_roundDuration / 60F);
        int seconds = Mathf.FloorToInt(_roundDuration % 60F);

        RoundData roundData = new RoundData
        {
            RoundDuration = $"{minutes:00}:{seconds:00}",
            MonstersSpawned = _currentRoundValue,  // Assuming this is the method that returns the total monsters spawned
            CurrentRound = _currentRound
        };

        roundDataList.roundsData.Add(roundData);

        string jsonData = JsonUtility.ToJson(roundDataList);
        
        // Use saveFilePath instead of savePath
        File.WriteAllText(saveFilePath, jsonData);
    }

    public int GetCurrentRound()
    {
        return _currentRound;
    }

    public float GetDelayBeforeNextRound()
    {
        return _delayBeforeNextRound;
    }

    public int GetPreviousRoundValue()
    {
        return _previousRoundValue;
    }

    public int GetCurrentRoundValue()
    {
        return _currentRoundValue;
    }
    

    #region test Purpose
    public void DeleteSaveOnEditorQuit()
    {
#if UNITY_EDITOR
        if(!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            DeleteSaveFile();
        }
#endif
    }

    public void DeleteSaveFile()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, _savePath);
        if(File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }
    #endregion  
}
