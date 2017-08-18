using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Enemy : NetworkBehaviour
{

    [SyncVar]
    public float health = 1;

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

    [Command]
    public virtual void CmdTakeDamage()
    {

    }

    [Server]
    public virtual void SrvTargetSomething()
    {

    }

    [Command]
    public virtual void CmdDie()
    {

    }

    [ClientRpc]
    public virtual void RpcDie()
    {

    }
}
