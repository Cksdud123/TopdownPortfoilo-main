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

        // 플레이어에 부착된 WeaponManager 컴포넌트를 찾기
        WeaponManager playerWeaponManager = other.GetComponentInChildren<WeaponManager>();

        playerWeaponManager.ammo.extraAmmo += Random.Range(1,5);

        AmmoUI.instance.UpdateAmmoText(playerWeaponManager.ammo.currentAmmo);
        AmmoUI.instance.UpdateMagText(playerWeaponManager.ammo.extraAmmo);

        gameObject.SetActive(false);
        // 또는 Destroy(gameObject);
    }
}
