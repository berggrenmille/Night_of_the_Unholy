using UnityEngine.Networking;
using UnityEngine;

public class Weapon : NetworkBehaviour{

    string weaponName;

    public float damage;
    public float fireRate;
    public float reloadTime;

    [SerializeField]
    protected LayerMask mask;

    public int clipSize;
    public int currentClipAmmo;
    public int maxAmmo;
    public int totalAmmo;

    public bool isAbleToShoot;
    public bool isReloading;
    public bool isShooting;

    public GameObject Model;
    public Transform gunBarrel;
    public Camera cam;
    public PlayerWeaponManager pwm;


    virtual public void Start()
    {

    }

    virtual public void Shoot()
    {

    }

    virtual public void StopShooting()
    {

    }

    virtual public void Reload()
    {

    }

    virtual public void AddAmmo(int amount)
    {

    }

    virtual public void Unequip()
    {

    }
}
