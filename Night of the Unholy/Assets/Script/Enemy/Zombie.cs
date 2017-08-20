using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Zombie : Enemy {
    
    private NavMeshAgent navAgent;
    private Rigidbody zombieRb;
    private NetworkInstanceId netIdentity;
    private IEnumerator targetPlayerCoRo;

    public Player nearestPlayer;
    public float nearestPlayerRange;

    public override void Start()
    {
        name = "Zombie";
        zombieRb = GetComponent<Rigidbody>();
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
                StartCoroutine(targetPlayerCoRo);
                break;
            default:
                StopCoroutine(targetPlayerCoRo);
                StartCoroutine(targetPlayerCoRo);
                break;        
        }
    }

    public override void RpcTakeDamage(float amount, int colliderId)
    {
        /*
         * colliderId = which body part was hit
         * 1 = head
         * 2 = chest
         * 3 = lArm
         * 4 = rArm
         * 5 = lLeg
         * 6 = rLeg
         * */
        switch (colliderId) // multiply damage depending on which body part you hit
        {
            case 1: //head
                amount *= 1.9f;
                break;
            case 2: //chest
                amount *= 1.2f;
                break;
            case 3: //arm
                amount *= 1.1f;
                break;
            case 4: //arm
                amount *= 1.1f;
                break;
            default:
                amount *= 1.0f;
                break;
        }

        

        if (!isDead)
        {
            health -= amount;
        }
    }

    public override void CmdDie() //call die for server
    {
        RpcDie();
    }

    public override void RpcDie() //call die for all clients
    {
        this.StopAllCoroutines();
        isDead = true;
        navAgent.enabled = false;
        zombieRb.constraints = RigidbodyConstraints.None;
    }

    public IEnumerator TargetPlayerCoRo(float waitTime) //Check for closest player
    {
        var count = 0;

        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            foreach (var _player in GameManager.currentInstance.players) //Check distance to all alive players
            {
                Player player = NetworkServer.FindLocalObject(_player.netId).GetComponent<Player>(); //Find the player script
                nearestPlayerRange = target == null ? 0f : (target.transform.position - transform.position).magnitude;
                if (!player.isDead)
                {
                    var range = (player.transform.position - transform.position).magnitude;
                    if (range < nearestPlayerRange || nearestPlayerRange == 0f)
                    {
                        nearestPlayer = player;
                        nearestPlayerRange = range;
                    }
                }
            }

            if (nearestPlayer != null) //Check if nearestplayer is visible
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, nearestPlayer.transform.position - transform.position, out hit,Mathf.Infinity))
                {
                    if (!hit.collider.CompareTag("Player"))
                    {
                        Debug.Log(name + " is tracking target without sight");
                        count++; //count ticks since lost view of player
                        continue;
                    }
                    else if(target != nearestPlayer.gameObject)
                    {
                            target = nearestPlayer.gameObject;
                            Debug.Log("New target: " + nearestPlayer);
                    }
                }
            }
        }
    }
}
