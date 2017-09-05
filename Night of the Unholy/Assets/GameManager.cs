using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;


public class GameManager : NetworkBehaviour
{
    public static GameManager currentInstance;
    public Gamemode gameMode;
    public class Players : SyncListStruct<Player.PlayerInfo>
    { }
    

    private void PlayersChanged(SyncListStruct<Player.PlayerInfo>.Operation op, int itemIndex)
    {
        Debug.Log("Playerlist operation: " + op +" " + players[itemIndex]);
    }

    public Players players; //The player list

    public override void OnStartServer()
    {
        StartCoroutine("SrvCheckForDisconnectedPlayersCoRo");
        base.OnStartServer();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (currentInstance != null)
        {
            Debug.LogError("More than one GameManager in scene! Replacing...");
            Destroy(currentInstance.gameObject);
            currentInstance = this;
        }
        else
        {
            currentInstance = this;
        }
        if (players == null) players = new Players();
        players.Callback = PlayersChanged;

        gameMode = gameObject.AddComponent<GamemodeZombieCoop>();
    }

    [Command]
    public void CmdEnemyTargetRandomPlayer(NetworkInstanceId enemyId) //return random player
    {
        if(players.Count > 0)
        {

            NetworkServer.FindLocalObject(enemyId).GetComponent<Enemy>().target = NetworkServer.FindLocalObject(players[UnityEngine.Random.Range(0, players.Count)].netId);
            //return NetworkServer.FindLocalObject(players[UnityEngine.Random.Range(0, players.Count)].netId);
        }
        else
        {
            NetworkServer.FindLocalObject(enemyId).GetComponent<Enemy>().target = null;
            //return null;
        }
    }

    public void AddPlayerToList(Player.PlayerInfo p_info)
    {
        var info = p_info;
        players.Add(info);
    }

    public void RemovePlayerFromList(Player.PlayerInfo obj)
    {
        if (players.Contains(obj))
            players.Remove(obj);
    }

    private IEnumerator SrvCheckForDisconnectedPlayersCoRo()
    {
        while (NetworkServer.active)
        {
            foreach (var info in players)
            {
                if (!NetworkServer.FindLocalObject(info.netId))
                {
                    RemovePlayerFromList(info);
                    break;
                }
            }
            yield return new WaitForSeconds(5);
        }
    }
}



