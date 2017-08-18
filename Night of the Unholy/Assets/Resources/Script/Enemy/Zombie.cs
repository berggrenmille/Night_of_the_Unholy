using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine;

public class Zombie : Enemy {
    
    private NavMeshAgent navAgent;
    private NetworkInstanceId netIdentity;
    private IEnumerator targetPlayerCoRo;

    public Player nearestPlayer;
    public float nearestPlayerRange;

    public override void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        netIdentity = GetComponent<NetworkIdentity>().netId;
        targetPlayerCoRo = TargetPlayerCoRo(1f);

        nearestPlayer = target == null ? null : target.GetComponent<Player>();
        nearestPlayerRange = target == null ? 0f : (target.transform.position - transform.position).magnitude;
}

    // Update is called once per frame
    public override void Update () {
        if (isServer)
        {
            if (target == null)
                SrvTargetSomething();
            else
                navAgent.destination = target.transform.position;
        }
    }

    public override void attack()
    {

    }

    public override void SrvTargetSomething()
    {
        int randomNum = Random.Range(0, 5); //pick what to target
        switch (randomNum)
        {
            case 1:
                StopCoroutine(targetPlayerCoRo);
                GameManager.currentInstance.CmdZombieTargetRandomPlayer(netIdentity); // target random player
                StartCoroutine(targetPlayerCoRo);
                break;
            default:
                StopCoroutine(targetPlayerCoRo);
                StartCoroutine(targetPlayerCoRo);
                break;        
        }
    }

    public override void CmdTakeDamage()
    {

    }

    public override void CmdDie()
    {
        
    }

    public override void RpcDie()
    {

    }

    public IEnumerator TargetPlayerCoRo(float waitTime) //Check for closest player
    {
        var count = 0;
       

        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            foreach (var _player in GameManager.currentInstance.players) //Check distance to all alive players
            {
                Debug.Log(_player.ToString());
                Player player = NetworkServer.FindLocalObject(_player.netId).GetComponent<Player>(); //Find the player script
                nearestPlayerRange = target == null ? 0f : (target.transform.position - transform.position).magnitude;
                if (!player.isDead)
                {
                    var range = (player.transform.position - transform.position).magnitude;
                    if (range < nearestPlayerRange || nearestPlayerRange == 0f)
                    {
                        Debug.Log("New nearest player");
                        nearestPlayer = player;
                        nearestPlayerRange = range;
                    }

                }
            }

            if (nearestPlayer != null) //Check if nearestplayer is visible
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, nearestPlayer.transform.position - transform.position, out hit,
                    Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        target = nearestPlayer.gameObject;
                        Debug.Log("Targeting: " + nearestPlayer);
                    }
                    else
                    {
                        Debug.Log("Folling player without sight");
                        count++;
                        continue;
                    }
                }
                else
                {
                    Debug.Log("Folling player without sight");
                    count++;
                    continue;
                }

            }
        }
    }
}
