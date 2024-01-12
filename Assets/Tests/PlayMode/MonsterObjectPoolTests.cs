using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MonsterObjectPoolTests
{
    
    [UnityTest]
    public IEnumerator GetMonsterFromPool_ReturnsValidMonster()
    {
        // Create an empty GameObject to hold the MonsterObjectPool script
        GameObject poolObject = new GameObject();
        MonsterObjectPool monsterPool = poolObject.AddComponent<MonsterObjectPool>();

        // Manually set the monster prefab (replace with your actual monster prefab)
        monsterPool.MonsterPrefab = new GameObject();

        // Get a monster from the pool
        GameObject monster = monsterPool.GetMonsterFromPool();

        // Ensure the returned monster is not null
        Assert.IsNotNull(monster);

        // Ensure the returned monster is inactive
        Assert.IsFalse(monster.activeSelf);

        // Clean up
        Object.Destroy(poolObject);

        // Yield to the next frame
        yield return null;
    }

    [UnityTest]
    public IEnumerator ReturnToPool_DeactivatesMonster()
    {
        GameObject monsterPrefab = new GameObject();
        MonsterObjectPool monsterObjectPool = new GameObject().AddComponent<MonsterObjectPool>();
        monsterObjectPool.MonsterPrefab = monsterPrefab;
        GameObject monster = monsterObjectPool.GetMonsterFromPool();

        // Act
        monsterObjectPool.ReturnToPool(monster);
        Assert.IsFalse(monster.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ReturnToPool_WithInvalidMonster_DoesNotThrowError()
    {
        // Arrange
        MonsterObjectPool monsterObjectPool = new GameObject().AddComponent<MonsterObjectPool>();
        GameObject invalidMonster = new GameObject(); 

        // Act & Assert
        Assert.DoesNotThrow(() => monsterObjectPool.ReturnToPool(invalidMonster));

        yield return null;
    }
}
