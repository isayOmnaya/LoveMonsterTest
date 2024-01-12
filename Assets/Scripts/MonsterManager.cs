
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    List<Monster> _activeMonsters = new List<Monster>();

    [SerializeField]
    CameraSettings _cameraSettings = null;

    [SerializeField]
    RoundManager _roundManager = null;

    [SerializeField]
    Transform _spawnPoint = null;

    [SerializeField]
    GuiManager _guiManager = null;

    MonsterObjectPool _monsterPool = null;

    [Header("Data")]
    int _totalNumberOfMonstersCount = 0;



    public void Initialize(MonsterObjectPool monsterObjectPool)
    {
        _monsterPool = monsterObjectPool;
    }

    //going with a topdown setup where i call updateExternal in the update of a GameAuthority script;
    //idea is to have a single monobehavior that calls Starts, Update etc
    public void UpdateExternal()
    {
        UpdateActiveMonsters(_monsterPool, _roundManager);
        _guiManager.UpdateMonsterCount(_totalNumberOfMonstersCount);
        _cameraSettings.UpdateCameraSpawnPoints(_spawnPoint);
    }

    void UpdateActiveMonsters(MonsterObjectPool monsterPool, RoundManager roundManager)
    {
        for(int i = _activeMonsters.Count - 1; i >= 0; i--)
        {
            if(_activeMonsters[i].gameObject.activeSelf)
            {
                _activeMonsters[i].UpdateExternal();
            }
            else
            {
                monsterPool.ReturnToPool(_activeMonsters[i].gameObject);
                _activeMonsters.RemoveAt(i);
            }
        }

       
        // Check if all monsters have left the screen
        if(_activeMonsters.Count == 0)
        {
            roundManager.DelayBeforeSpawn(this, _spawnPoint);
        }
    }

    public bool CheckMonsterInScreen()
    {
       return _activeMonsters.Count == 0;
    }

    public void AddMonster(Monster monster)
    {
        _activeMonsters.Add(monster);
        _totalNumberOfMonstersCount++;
    }

    public int GetMonsterCount()
    {
        return _totalNumberOfMonstersCount;
    }
}
