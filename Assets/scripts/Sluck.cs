using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sluck : Monster
{
    public GameObject coinPrefab;

    private int coinLowerBound = 1;
    private int coinUpperBound = 3;

    protected override void DestroyUnit()
    {
        int numCoinsDrop = UnityEngine.Random.Range(coinLowerBound, coinUpperBound);
        Vector3 dropPosition = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        for (int i = 0; i < numCoinsDrop; i++)
        {
            Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        }
        base.DestroyUnit();
    }
}
