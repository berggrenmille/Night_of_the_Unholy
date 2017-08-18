using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Pistol : Weapon
{
    [Client]
    public override void Shoot()
    {
        if (isAbleToShoot && !isReloading)
        {
            isShooting = true;
            isAbleToShoot = false;
            StartCoroutine("ShootCoRo");
        }
    }

    public override void StopShooting()
    {
        isShooting = false;
    }

    public override void Reload()
    {
        if (!isReloading)
        {
            if (currentClipAmmo < clipSize && totalAmmo > 0)
            {
                Debug.Log("Reloading...");
                isReloading = true;
                StartCoroutine("ReloadCoRo");
            }
        }
    }

    public override void AddAmmo(int amount)
    {
        if (totalAmmo + amount <= maxAmmo)
        {
            totalAmmo += amount;
        }
        else
        {
            totalAmmo += maxAmmo - totalAmmo;
        }
    }

    public override void Unequip()
    {

    }

    IEnumerator ReloadCoRo()
    {
        yield return new WaitForSeconds(reloadTime);
        if (totalAmmo >= clipSize - currentClipAmmo)
        {
            totalAmmo -= clipSize - currentClipAmmo;
            currentClipAmmo += clipSize - currentClipAmmo;
        }
        else if (totalAmmo < clipSize - currentClipAmmo)
        {
            currentClipAmmo += totalAmmo;
            totalAmmo = 0;
        }
        isReloading = false;
        isAbleToShoot = true;
        yield break;
    }

    [Client]
    IEnumerator ShootCoRo()
    {
        if (currentClipAmmo != 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, mask))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    pwm.CmdPlayerShotEnemy(hit.collider.name, hit.collider.transform.root.gameObject, damage);
                }
            }
            currentClipAmmo--;
            while (isShooting)
            {
                yield return null;
            }
            yield return new WaitForSeconds(1 / fireRate);
            isAbleToShoot = true;
        }
        else
        {
            isAbleToShoot = false;
        }
        yield break;
    }
}


