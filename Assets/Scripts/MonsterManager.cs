using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    List<Monster> activeMonsters = new List<Monster>();

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
    int _monsterCount = 0;



    public void Initialize(MonsterObjectPool monsterObjectPool)
    {
        _monsterPool = monsterObjectPool;
        _cameraSettings.InitializeSpawnPoint(_spawnPoint); // can call in updateExternal too
    }

    public void UpdateExternal()
    {
        UpdateExternal(_monsterPool, _roundManager);
        _guiManager.UpdateMonsterCount(_monsterCount);
         _cameraSettings.InitializeSpawnPoint(_spawnPoint);
    }

    void UpdateExternal(MonsterObjectPool monsterPool, RoundManager roundManager)
    {
        for(int i = 0; i < activeMonsters.Count; i++)
        {
            if(activeMonsters[i].gameObject.activeSelf)
            {
                activeMonsters[i].UpdateExternal();
            }
            else
            {
                monsterPool.ReturnToPool(activeMonsters[i].gameObject);
                activeMonsters.RemoveAt(i);
                i--;
            }
        }

        // Check if all monsters have left the screen
        if(activeMonsters.Count == 0)
        {
            roundManager.DelayBeforeSpawn(this, _spawnPoint);
        }
    }

    public bool CheckMonsterInScreen()
    {
       return activeMonsters.Count == 0;
    }

    public void AddMonster(Monster monster)
    {
        activeMonsters.Add(monster);
        _monsterCount++;
    }

    public int GetMonsterCount()
    {
        return _monsterCount;
    }
}
