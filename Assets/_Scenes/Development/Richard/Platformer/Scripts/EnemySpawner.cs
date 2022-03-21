using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyprefab;
    [SerializeField] Transform spawnLocation;

    [SerializeField] bool respawnOnDeath;
    [SerializeField] float delayBeforeRespawn = 2.0f;

    void Awake()
    {
        EventBroker.EnemyKilled += OnEnemyKilled;
    }

    void Start()
    {
        SpawnEnemy();
    }

    void OnDestroy()
    {
        EventBroker.EnemyKilled -= OnEnemyKilled;
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyprefab, spawnLocation.position, Quaternion.identity);
    }
    
    private void OnEnemyKilled()
    {
        if (respawnOnDeath)
        {
            StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delayBeforeRespawn);
        SpawnEnemy();
    }
}
