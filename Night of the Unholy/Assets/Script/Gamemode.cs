using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

[System.Serializable]
public class Gamemode : NetworkBehaviour
{
    public new static string name;
    public static int maxPlayers;

    public GameObject[] enemyPrefabs; //All enemies that will be able to spawn
    public GameObject[] enemySpawnpoints; //Where enemies will be able to spawn


    protected virtual void Start()
    {

    }

    public virtual void SetupGamemode()
    {
        Debug.Log("Setting up gamemode");
    }
}
