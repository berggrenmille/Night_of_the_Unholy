using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class Player : NetworkBehaviour
{
    public struct PlayerInfo
    {
        public string name;
        public float added;
        public NetworkInstanceId netId;
        public override string ToString()
        {
            return name + " (" + netId + ")";
        }
    };

    public static int playerCount = 0;

    [System.Serializable]
    public class Movement
    {
        public float moveSpeedForwards = 10.0f;
        public float moveSpeedBackwards = 5.0f;
        public float strafeSpeed = 3.0f;
        public float moveSpeedMultiplier = 1.0f;
        public float maxVelocityChange = 1.0f;

        public float jumpForce = 1.0f;

        public bool isRunning = false;
    }

    [SyncVar]
    public float health = 1;
    public float healthMax = 50;
    public float healthRegenPerSecond = 2;

    [SyncVar]
    public bool isDead = false;
    public Behaviour[] disableWhenDead;
    public GameObject[] disableGameObjectsWhenDead;
    public bool[] wasEnabled;

    public Movement movement;

    private float distToGround;

    public Settings settings;
    public PlayerInfo info;

    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        GetComponent<PlayerSetup>().NewSetup();
    }

    // Use this for initialization
    void Start()
    {
        settings = Settings.GetInstance();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void Die() //Setup for death
    {

    }

    public bool isGrounded() //Check if player is grounded
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }


    [ClientRpc]
    public void RpcAddHealth(int amount) //Add health to health pool
    {
        if (health + amount > healthMax)
        {
            health = healthMax;
        }
        else
        {
            health += amount;
        }
    }

    [ClientRpc]
    public void RpcRemoveHealth(int amount) //Remove health from health pool
    {
        if (!isDead && health - amount < health)
        {
            health = 0;
            Die();
        }
        else
        {
            health -= amount;
        }
    }

}

