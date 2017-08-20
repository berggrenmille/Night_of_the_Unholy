using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeZombieCoop : Gamemode {
    //wave class
    [SerializeField]
    public  WaveManager waveManager;

    protected override void Awake()
    {
        waveManager = gameObject.AddComponent<WaveManager>();
    }
    public override void SetupGamemode()
    {
        base.SetupGamemode();

    }
}
