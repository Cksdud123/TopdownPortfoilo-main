using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // �÷��̾ ������ WeaponManager ������Ʈ�� ã��
        WeaponManager playerWeaponManager = other.GetComponentInChildren<WeaponManager>();

        playerWeaponManager.ammo.extraAmmo += Random.Range(1,5);

        AmmoUI.instance.UpdateAmmoText(playerWeaponManager.ammo.currentAmmo);
        AmmoUI.instance.UpdateMagText(playerWeaponManager.ammo.extraAmmo);

        gameObject.SetActive(false);
        // �Ǵ� Destroy(gameObject);
    }
}
