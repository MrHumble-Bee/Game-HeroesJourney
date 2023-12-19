using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkMonster : Monster
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

    protected override void Wander(Vector3 randomDirection)
    {
        animator.SetBool("IsRunning", false);
        randomDirection.y = 0; // Keep the direction in the horizontal plane

        // Calculate the target position to move towards in the random direction
        Vector3 targetPosition = transform.position + randomDirection;

        // Move the Monster towards the random direction
        rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, currentSpeed / 3 * Time.deltaTime));

        // Rotate the Monster to face the random direction
        Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 180.0f * Time.deltaTime));

    }
}
