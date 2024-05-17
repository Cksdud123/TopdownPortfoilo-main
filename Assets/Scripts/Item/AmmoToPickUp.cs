using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoToPickUp : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        WeaponManager playerWeaponManager = other.GetComponentInChildren<WeaponManager>();

        if (playerWeaponManager.weaponState == Weapon.Knife)
        {
            return;
        }
        else
        {
            playerWeaponManager.ammo.extraAmmo += Random.Range(1, 5);

            AmmoUI.instance.UpdateAmmoText(playerWeaponManager.ammo.currentAmmo);
            AmmoUI.instance.UpdateMagText(playerWeaponManager.ammo.extraAmmo);

            gameObject.SetActive(false);
        }
    }
}
