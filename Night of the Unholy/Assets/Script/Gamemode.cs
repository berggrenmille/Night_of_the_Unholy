using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Gamemode : NetworkBehaviour
{
    public new string name;
    public int maxPlayers;

    protected virtual void Awake()
    {
        
    }

    public virtual void SetupGamemode()
    {
        Debug.Log("Setting up gamemode");
    }
}
