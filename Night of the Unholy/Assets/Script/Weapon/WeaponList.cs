using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponList : MonoBehaviour {

    public static WeaponList currentInstance;

    public enum Weapons
    {
        /*
         * Give all weapons ids by storing their names in an enum.
         * Weapons must have the same name as its filename (Capital letters does not matter)
         */
        PISTOL = 0, LENGTH
    }
    public List<GameObject> weaponsPrefabs = new List<GameObject>();

    private void Setup() //Load weapons into memory
    {
        for (int i = 0; i < (int)Weapons.LENGTH; i++)
        {
            string weaponPathName = ((Weapons) i).ToString().ToLower();
            if (string.IsNullOrEmpty(weaponPathName))
                throw new ArgumentException("weapon name is null");
            weaponPathName = weaponPathName.First().ToString().ToUpper() + weaponPathName.Substring(1); //
            weaponsPrefabs.Add(Resources.Load("Prefab/Weapon/" + weaponPathName) as GameObject);
        }
    }

    public Weapon GetWeapon(Weapons id)
    {
        return weaponsPrefabs[(int)id].GetComponent<Weapon>();
    }

    public Weapon GetWeapon(int id)
    {
        return weaponsPrefabs[id].GetComponent<Weapon>();
    }

    public static WeaponList GetInstance()
    {
        if(currentInstance == null)
        {
            GameObject go = new GameObject();
            go.name = "WeaponList instance";
            currentInstance = go.AddComponent<WeaponList>();
            currentInstance.Setup();
        }
        return currentInstance;
    }
}
