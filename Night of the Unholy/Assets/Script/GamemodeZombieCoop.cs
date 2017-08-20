using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamemodeZombieCoop : Gamemode {
    //wave class
    [SerializeField]
    public WaveManager waveManager;



    protected override void Start()
    {
        waveManager = gameObject.AddComponent<WaveManager>();
        enemyPrefabs = Resources.LoadAll<GameObject>("Prefab/Enemy");
        enemySpawnpoints = GameObject.FindGameObjectsWithTag("SpawnEnemy");
        waveManager.SetupWaves();
    }
    public override void SetupGamemode()
    {
        base.SetupGamemode();

    }
}
