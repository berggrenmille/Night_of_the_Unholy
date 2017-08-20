using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Enemy : NetworkBehaviour
{

    [SyncVar] public new string name = "enemy";

    [SyncVar] public float health = 1;

    [SyncVar] public bool isDead = false;

    public float damage = 1;

    public GameObject target;

    [Server]
    public virtual void Start()
    {
        
    }
    [Server]
    public virtual void Update()
    {
        
    }

    [Server]
    public virtual void attack()
    {

    }

    [ClientRpc]
    public virtual void RpcTakeDamage(float amount, int colliderId)
    {
 
    }

    [Server]
    public virtual void SrvTargetSomething()
    {

    }

    [Command]
    public virtual void CmdDie()
    {
        Debug.Log(name + " died");
    }

    [ClientRpc]
    public virtual void RpcDie()
    {

    }
}
