using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Numerics;

public class RoundManagerTests 
{
    static bool IsFibonacci(BigInteger n)
    {
        return IsPerfectSquare(5 * n * n + 4) || IsPerfectSquare(5 * n * n - 4);
    }

    static bool IsPerfectSquare(BigInteger num)
    {
        BigInteger sqrt = Sqrt(num);
        return sqrt * sqrt == num;
    }

    static BigInteger Sqrt(BigInteger value)
    {
        if (value == 0 || value == 1)
            return value;

        BigInteger start = 1, end = value / 2, result = 0;

        while (start <= end)
        {
            BigInteger mid = (start + end) / 2;
            if (mid * mid == value)
                return mid;

            if (mid * mid < value)
            {
                start = mid + 1;
                result = mid;
            }
            else
            {
                end = mid - 1;
            }
        }

        return result;
    }

    [UnityTest]
    public IEnumerator CalculateNextFibonacciValue_CalculatesCorrectly()
    {
        // Arrange
        RoundManager roundManager = new GameObject().AddComponent<RoundManager>();

        //anything bigger than 45 will actually throw an error
        for (int i = 1; i < 45; i++)
        {
            roundManager.CalculateNextFibonacciValue();
            BigInteger expectedValue = roundManager.GetCurrentRoundValue();
            Assert.IsTrue(IsFibonacci(expectedValue), $"Failed for round {i}");
            yield return null; 
        }
    }
}
