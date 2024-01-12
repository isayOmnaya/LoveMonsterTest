using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoundManagerTests 
{
    
    [UnityTest]
    public IEnumerator CalculateNextFibonacciValue_CalculatesCorrectly()
    {
        // Arrange
        RoundManager roundManager = new GameObject().AddComponent<RoundManager>();

        // Act
        roundManager.CalculateNextFibonacciValue();

        // Assert
        Assert.AreEqual(1, roundManager.GetPreviousRoundValue());
        Assert.AreEqual(1, roundManager.GetCurrentRoundValue());

        yield return null;
    }
}
