using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : GameRuleInteractor<GameRules> {

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject arena;

    // Spawn timer
    [SerializeField] private float initalSpawnTime = 5;
    [SerializeField] private float spawnTimeDecreaseInterval = 10;
    [SerializeField] private float spawnTimeDecreaseAmount = 1;
    [SerializeField] private float spawnTimeMin = 1;
    private float currentSpawnTime;
    private float lastSpawnTime = 0;

    // Spawn count
    //[SerializeField] private float initialSpawnCount = 1;
    //[SerializeField] private float spawnCountIncrease = 1;
    //private float currentSpawnCount;

    // WaveConfigs
    [SerializeField] private WaveConfig[] waveConfigs;
    private int currentWaveConfigIdx = 0;
    private float currentWaveConfigStart = 0;

    //private float roundStartTime;

    private Vector3 arenaCenter;
    
	void Start () {
        //roundStartTime = Time.time;
        arenaCenter = arena.transform.position;
        SpawnBlob();
	}
	
	void Update () {
        Spawn();
	}

    void Spawn() {
        float spawnReduction = Mathf.Floor((Time.time - gameRules.RoundStartTime) / spawnTimeDecreaseInterval) * spawnTimeDecreaseAmount;
        float spawnTime = Mathf.Max(spawnTimeMin, initalSpawnTime - spawnReduction);
        if (Time.time - lastSpawnTime > spawnTime) {
            SpawnBlobs(2);
        }
    }

    void SpawnBlobs(int count) {
        for (int i = 0; i < count; i++) {
            SpawnBlob();
        }
    }

    void SpawnBlob() {
        Vector2 rndPoint = Random.insideUnitCircle;
        Vector3 spawnDirection = new Vector3(rndPoint.x, 0, rndPoint.y);
        Vector3 spawn = arenaCenter + spawnDirection.normalized * cam.orthographicSize * 2.5f;
        float i = Random.value;
        GameObject prefab = ChooseEnemyType();
        GameObject blob = Instantiate(prefab, spawn, new Quaternion());

        rndPoint = Random.insideUnitCircle;
        blob.transform.LookAt(new Vector3(rndPoint.x,0,rndPoint.y)*cam.orthographicSize);

        lastSpawnTime = Time.time;
    }

    GameObject ChooseEnemyType() {
        WaveConfig currentWaveConfig = waveConfigs[currentWaveConfigIdx];
        // Check if we need to increase the wave config index.
        if(currentWaveConfigIdx != waveConfigs.Length - 1 && 
            currentWaveConfig.maxDuration < Time.time - currentWaveConfigStart) {

            currentWaveConfigIdx += 1;
            currentWaveConfigStart = Time.time;
        }
        
        WaveConfig.WaveConfigEntry[] entries = currentWaveConfig.waveConfigs;

        // Normalize spawnrates.
        float sum = 0;
        foreach (WaveConfig.WaveConfigEntry entry in entries) {
            sum += entry.spawnChance;
        }
        float rnd = Random.Range(0.0f, sum);
        
        // Check chances.
        float accumalateChance = 0;
        for (int i = 0; i < entries.Length; i++) {
            accumalateChance += entries[i].spawnChance;
            if (rnd <= accumalateChance) {
                return gameRules.BlobPrefabs[i];
            }
        }

        // This should never happen.
        throw new System.Exception();
    }
}
