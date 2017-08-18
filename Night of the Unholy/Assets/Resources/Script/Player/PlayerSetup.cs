using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour {

    public Behaviour[] objectsToDisable;
    private bool firstSetup = true;
    private Player player;
    void Start () {

        if (!isLocalPlayer) //turn of components for other players
        {
            foreach (Behaviour item in objectsToDisable)
            {
                item.enabled = false;
            }
        }else if(isLocalPlayer)
        {
            NewSetup();
            CmdAddPlayerToServerList();
        }
    }

    public void NewSetup()
    {
        CmdNewPlayerSetup();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            CmdActivateSetupOnPlayer(player);
            //player.GetComponent<PlayerSetup>().Setup();
            CmdActivateWeaponOnPlayer(player);
        }
    }

    [Command]
    public void CmdNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
        DefaultSetup();
    }

    public void DefaultSetup() //Set player default values, case respawning etc
    {
        player = GetComponent<Player>();
        player.isDead = false;
        player.health = player.healthMax;

        //Enable the components
        for (int i = 0; i < player.disableWhenDead.Length; i++)
        {
            player.disableWhenDead[i].enabled = player.wasEnabled[i];
        }

        //Enable the gameobjects
        for (int i = 0; i < player.disableGameObjectsWhenDead.Length; i++)
        {
            player.disableGameObjectsWhenDead[i].SetActive(true);
        }        player.isDead = false;

        
        player.info.netId = GetComponent<NetworkIdentity>().netId;
        player.info.name = "Player";
        
        GetComponent<PlayerWeaponManager>().Setup();
    }

    [Client]
    public void Setup() //Setup for other players
    {
        CmdSetup();
    }

    [Command]
    public void CmdSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    public void RpcSetupPlayerOnAllClients()
    {
        player = GetComponent<Player>();
        if (firstSetup)
        {
            player.wasEnabled = new bool[player.disableWhenDead.Length];
            for (int i = 0; i < player.wasEnabled.Length; i++)
            {
                player.wasEnabled[i] = player.disableWhenDead[i].enabled;
            }

            GetComponent<PlayerWeaponManager>().Setup();
            player.info.netId = GetComponent<NetworkIdentity>().netId;
            player.info.name = "Player";
            
            firstSetup = false;
        }
    }

    [Command]
    void CmdAddPlayerToServerList()
    {
        GameManager.currentInstance.AddPlayerToList(player.info);
    }

    [Command]
    void CmdActivateWeaponOnPlayer(GameObject p)
    {
        p.GetComponent<PlayerWeaponManager>().RpcActivateWeapon();
    }
    
    [Command]
    void CmdActivateSetupOnPlayer(GameObject p)
    {
        p.GetComponent<PlayerSetup>().RpcSetupPlayerOnAllClients();
    }

}
