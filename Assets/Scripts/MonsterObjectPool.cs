using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPool : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    GameObject _monsterPrefab;

    List<GameObject> _monsterList = new List<GameObject>();

    [Header("Data")]
    [SerializeField]
    int _threadHold = 0;

    public GameObject GetMonsterFromPool()
    {
        if(_monsterList.Count > 0)
        {
            for(int i = 0; i < _monsterList.Count; i++)
            {
                if(!_monsterList[i].activeInHierarchy)
                {
                    return _monsterList[i];
                }
            }
        }

        if (_monsterList.Count < _threadHold)
        {
            GameObject monster = Instantiate(_monsterPrefab, transform);
            monster.SetActive(false);
            _monsterList.Add(monster);
            return monster;
        }
        return null;
    }

    public void ReturnToPool(GameObject monster)
    {
        if(_monsterList.Contains(monster))
        {
            monster.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Trying to return a monster to the pool that doesn't belong to this pool.");
        }
    }

    public GameObject GetMonsterPrefab
    {
        get {return _monsterPrefab;}
        set {_monsterPrefab = value;}
    }
}
