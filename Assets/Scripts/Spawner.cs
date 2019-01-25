using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : GameRuleInteractor<GameRules> {

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject greenPrefab;

    [SerializeField] private float initalSpawnTime = 5;
    [SerializeField] private float decreaseTime = 10;
    [SerializeField] private float decreaseAmount = 1;
    [SerializeField] private float spawnTimeMin = 1;

    private float roundStartTime;

    private float currentSpawnTime;

    private float lastSpawnTime = 0;

    private Vector3 arenaCenter;

	// Use this for initialization
	void Start () {
        roundStartTime = Time.time;
        arenaCenter = arena.transform.position;
        SpawnBlob();
	}
	
	// Update is called once per frame
	void Update () {
        float spawnReduction = Mathf.Floor((Time.time - roundStartTime) / decreaseTime) * decreaseAmount;
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
        GameObject prefab;
        if (i > 0.88) {
            prefab = gameRules.BlobPrefabs[1];
        }
        else {
            prefab = gameRules.BlobPrefabs[0];
        }
        GameObject blob = Instantiate(prefab, spawn, new Quaternion());

        rndPoint = Random.insideUnitCircle;
        blob.transform.LookAt(new Vector3(rndPoint.x,0,rndPoint.y)*cam.orthographicSize);

        lastSpawnTime = Time.time;
    }
}
