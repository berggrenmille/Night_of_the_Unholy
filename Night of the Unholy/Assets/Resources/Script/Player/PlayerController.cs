using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private PlayerMotor p_motor;
    private Player player;
    private PlayerWeaponManager p_weaponManager;
    private PlayerUI p_ui;

    Vector2 inputMovement;

    float inputMouseX = 0;
    float inputMouseY = 0;


private void Start()
    {
        p_motor = GetComponent<PlayerMotor>();
        player = GetComponent<Player>();
        p_weaponManager = GetComponent<PlayerWeaponManager>();
        p_ui = GetComponent<PlayerUI>();
    }
    // Update is called once per frame
    private void Update()
    {
        inputMovement.x = Input.GetAxisRaw("Horizontal");
        inputMovement.y = Input.GetAxisRaw("Vertical");

        if (player.settings.mouseSettings.raw)
        {
            inputMouseX += Input.GetAxisRaw("Mouse X") * player.settings.mouseSettings.sensitivity;
            if (player.settings.mouseSettings.inverted)
            {
                inputMouseY -= Input.GetAxisRaw("Mouse Y") * player.settings.mouseSettings.sensitivity;
            }
            else
            {
                inputMouseY += Input.GetAxisRaw("Mouse Y") * player.settings.mouseSettings.sensitivity;
            }
        }
        else
        {
            inputMouseX += Input.GetAxis("Mouse X") * player.settings.mouseSettings.sensitivity;
            if (player.settings.mouseSettings.inverted)
            {
                inputMouseY -= Input.GetAxis("Mouse Y") * player.settings.mouseSettings.sensitivity;
            }
            else
            {
                inputMouseY += Input.GetAxis("Mouse Y") * player.settings.mouseSettings.sensitivity;
            }
        }

        inputMouseY = Mathf.Clamp(inputMouseY, -80, 80);

        //Check keyboard inputs

        if(Input.GetButtonDown("Cancel"))
        {
            p_ui.TogglePause();
        }

        if(Input.GetButton("Sprint"))
        {
            if(player.isGrounded())
            {
                player.movement.isRunning = true;
            }
        }else
        {
            player.movement.isRunning = false;
        }

        if(Input.GetButtonDown("Jump"))
        {
            if(player.isGrounded())
            {
                p_motor.Jump();
            }
        }

        if(Input.GetButtonDown("Reload"))
        {
            if (p_weaponManager.currentWeapon != null)
            {
                p_weaponManager.currentWeapon.Reload();
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(p_weaponManager.currentWeapon != null)
            {
                p_weaponManager.currentWeapon.Shoot();
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (p_weaponManager.currentWeapon != null)
            {
                p_weaponManager.currentWeapon.StopShooting();
            }
        }

    }


    private void FixedUpdate()
    {
        p_motor.DoMovement(p_motor.CalculateMovement(inputMovement.normalized));
        p_motor.DoRotation(inputMouseX, inputMouseY);
    }




}
