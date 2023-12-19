using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;  // The monster prefab to be instantiated.
    public Transform spawnPoint;      // The point where monsters will be spawned.
    public float spawnRate = 2.0f;    // Spawn rate in monsters per second.
    public int maxMonsters = 10;      // Maximum number of monsters to spawn.

    private float nextSpawnTime;      // Time to spawn the next monster.
    private int spawnedMonsters;      // Number of monsters spawned so far.

    void Start()
    {
        nextSpawnTime = Time.time + 1.0f / spawnRate;
        spawnedMonsters = 0;
        while (spawnedMonsters < maxMonsters)
        {
            SpawnMonster();
        }
    }

    void Update()
    {
        spawnedMonsters = transform.childCount;
        if (Time.time >= nextSpawnTime && spawnedMonsters < maxMonsters)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + 1.0f / spawnRate;
        }
    }

    void SpawnMonster()
    {
        // Instantiate a monster at the spawn point.
        GameObject monster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
        monster.transform.SetParent(this.transform);
        spawnedMonsters++;
    }
}
