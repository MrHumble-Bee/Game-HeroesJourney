using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Hero
{
    public AudioSource darkSight;
    public GameObject leftClone;
    public GameObject rightClone;

    protected override void Start()
    {
        // Start invoking the function every 3 seconds
        InvokeRepeating("SpawnAndDisappearClones", 0f, 15f);
    }

    void SpawnAndDisappearClones()
    {
        // Spawn the clones with random delays
        StartCoroutine(SpawnAndDisappear(leftClone, Random.Range(1f, 5f))); // Adjust the range as needed
        StartCoroutine(SpawnAndDisappear(rightClone, Random.Range(1f, 5f))); // Adjust the range as needed
    }

    IEnumerator SpawnAndDisappear(GameObject clone, float delay)
    {
        // Wait for the random delay
        yield return new WaitForSeconds(delay);
        // Activate the clone
        clone.SetActive(true);

        // Wait for 1 second
        yield return new WaitForSeconds(12f);

        // Deactivate the clone
        darkSight.Play();
        clone.SetActive(false);
    }
}
