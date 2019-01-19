﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject greenPrefab;

    [SerializeField] private float initalSpawnTime = 5;
    [SerializeField] private float decreaseTime = 10;
    [SerializeField] private float decreaseAmount = 1;
    [SerializeField] private float spawnTimeMin = 1;

    private float currentSpawnTime;

    private float lastSpawnTime = 0;

    private Vector3 arenaCenter;

	// Use this for initialization
	void Start () {
        arenaCenter = arena.transform.position;
        SpawnBlob();
	}
	
	// Update is called once per frame
	void Update () {
        float spawnReduction = Mathf.Floor(Time.time / decreaseTime) * decreaseAmount;
        float spawnTime = Mathf.Max(spawnTimeMin, initalSpawnTime - spawnReduction);
        if (Time.time - lastSpawnTime > spawnTime) {
            SpawnBlob();
            SpawnBlob();
        }
	}

    void SpawnBlob() {
        Vector2 rndPoint = Random.insideUnitCircle;
        Vector3 spawnDirection = new Vector3(rndPoint.x, 0, rndPoint.y);
        Vector3 spawn = arenaCenter + spawnDirection.normalized * cam.orthographicSize * 2.5f;
        GameObject blob = Instantiate(greenPrefab, spawn, new Quaternion());

        rndPoint = Random.insideUnitCircle;
        blob.transform.LookAt(new Vector3(rndPoint.x,0,rndPoint.y)*cam.orthographicSize);

        lastSpawnTime = Time.time;
    }
}