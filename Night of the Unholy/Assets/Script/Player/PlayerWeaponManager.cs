using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerWeaponManager : NetworkBehaviour {
    private Player player;
    
    public enum WeaponInventory
    {
        PRIMARY, SECONDARY
    }

    private WeaponList weaponList; //list to look up weapons
    private WeaponInventory usingWeapon; //current weapon in inventory

    public Weapon[] weapons = new Weapon[2]; //the gun "inventory"
    public Weapon currentWeapon; //the current weapon
    public GameObject currentWeaponObj; //reference to weapon gameobject
    [SyncVar]
    public int syncWeaponId = 0;

    [SerializeField]
    private Transform gunHolder;

    public void Setup()
    {
        player = GetComponent<Player>();
        weaponList = WeaponList.GetInstance();
        usingWeapon = WeaponInventory.PRIMARY;
        weapons[(int)usingWeapon] = weaponList.GetWeapon(syncWeaponId);
       // ActivateWeapon();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActivateWeapon()
    {
        CmdActivateWeapon();
    }


    public void SwitchWeapon(WeaponInventory inventoryPlace)
    {
        usingWeapon = inventoryPlace;
        ActivateWeapon();
    }

    public void ReplaceWeapon(WeaponList.Weapons weaponId)
    {
        syncWeaponId = (int)weaponId;
        weapons[(int)usingWeapon] = (Weapon)weaponList.GetWeapon(weaponId);
        ActivateWeapon();
    }

    void SetRenderLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetRenderLayerRecursively(child.gameObject, newLayer);
        }
    }
    void SetRenderLayerRecursively(GameObject obj, string newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = LayerMask.NameToLayer(newLayer);

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetRenderLayerRecursively(child.gameObject, newLayer);
        }
    }

    [ClientRpc] // called on all client
    public void RpcActivateWeapon()
    {   
        if (currentWeapon != null) Destroy(currentWeaponObj); // destroy current weapon

        weapons[(int)usingWeapon] = weaponList.GetWeapon((WeaponList.Weapons)syncWeaponId); //Sync weapon for all players

        currentWeapon = Instantiate(weapons[(int)usingWeapon], Vector3.zero, gunHolder.rotation, gunHolder);
        currentWeaponObj = currentWeapon.gameObject;
        currentWeapon.pwm = this;
        currentWeaponObj = currentWeapon.gameObject;
        currentWeaponObj.transform.SetParent(gunHolder);
        currentWeapon.cam = GetComponentInChildren<Camera>();
        currentWeaponObj = currentWeapon.gameObject;
        currentWeaponObj.transform.parent = gunHolder;
        currentWeaponObj.transform.localPosition = Vector3.zero;

        if (isLocalPlayer) //Set render layer if is local player
        {
            SetRenderLayerRecursively(currentWeaponObj, "FPS");
        }
    }

    [Command] //Called on server only
    public void CmdActivateWeapon()
    {
        Debug.Log(player.info.ToString() + " activated his weapon");
        RpcActivateWeapon();
    }

    [Command] //Called on server only
    public void CmdPlayerShotEnemy(string collider, GameObject enemy, float damage)
    {
        Debug.Log(player.info.name + " shot " + enemy + " at " + collider);
        int colliderId;
        switch (collider)
        {
            case "head":
                colliderId = 1;
                break;
            case "chest":
                colliderId = 2;
                break;
            case "l_arm":
                colliderId = 3;
                break;
            case "r_arm":
                colliderId = 4;
                break;
            case "l_leg":
                colliderId = 5;
                break;
            case "r_leg":
                colliderId = 6;
                break;
            default:
                colliderId = 0;
                break;
        }
        var e = enemy.GetComponent<Enemy>();
        e.RpcTakeDamage(damage, colliderId);
    }
}

