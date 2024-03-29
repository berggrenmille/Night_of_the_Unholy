﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Player player;

    private Rigidbody playerRb;
    private Camera cam;


	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
        playerRb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Jump()
    {
        playerRb.AddForce(player.movement.jumpForce * transform.up, ForceMode.VelocityChange);
        playerRb.AddForce(player.movement.jumpForce * playerRb.velocity.normalized, ForceMode.VelocityChange);
    }


    public Vector3 CalculateMovement(Vector2 input)
    {
        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y).normalized;
        targetVelocity = transform.TransformDirection(targetVelocity);

        if (input.y > 0)
         {
             targetVelocity *= player.movement.moveSpeedForwards;
         }
         else if (input.y < 0)
         {
             targetVelocity *= player.movement.moveSpeedBackwards;
         }
         else
         {
             targetVelocity *= player.movement.strafeSpeed;
         }
        if(player.movement.isRunning)
        {
            return targetVelocity * player.movement.moveSpeedMultiplier * 1.4f;
        }
        else
        {
            return targetVelocity * player.movement.moveSpeedMultiplier;
        }
        



        /* old way
         if (input.y > 0)
         {
             return (transform.forward * input.y * player.movement.moveSpeedForwards
                     + transform.right * input.x * player.movement.strafeSpeed)
                     * player.movement.moveSpeedMultiplier * Time.fixedDeltaTime;
         }
         else if(input.y < 0)
         {
             return (transform.forward * input.y * player.movement.moveSpeedBackwards
                     + transform.right * input.x * player.movement.strafeSpeed)
                     * player.movement.moveSpeedMultiplier * Time.fixedDeltaTime;
         }
         else
         {
             return  transform.right * input.x * player.movement.strafeSpeed
                     * player.movement.moveSpeedMultiplier * Time.fixedDeltaTime;
         }
         */

    }


    public void DoMovement(Vector3 _targetVelocity)
    {
        //playerRb.AddForce(_velocity, ForceMode.VelocityChange);
        // playerRb.MovePosition(transform.position + _velocity);

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = playerRb.velocity;
        Vector3 velocityChange = (_targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -player.movement.maxVelocityChange, player.movement.maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -player.movement.maxVelocityChange, player.movement.maxVelocityChange);
        velocityChange.y = 0;

        playerRb.AddForce(velocityChange, ForceMode.VelocityChange);

    }

    public void DoRotation(float _x, float _y)
    {
        Quaternion targetRot = Quaternion.Euler(0, _x, 0);
        Quaternion targetRotCam = Quaternion.Euler(-_y, 0, cam.transform.rotation.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, player.settings.mouseSettings.smoothness);
        cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, targetRotCam, player.settings.mouseSettings.smoothness);

    }
}
