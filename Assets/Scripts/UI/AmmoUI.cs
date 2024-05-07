using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Image ringAmmoBar;

    public Text currentAmmoText;
    public Text extraAmmoText;

    private WeaponAmmo weaponAmmo;

    float lerpSpeed;

    public static AmmoUI instance;

    private void Awake()
    {
        instance = this;
        lerpSpeed = 3f;
    }

    public void AmmoBarFilter(int currentAmmo,int clipsize)
    {
        ringAmmoBar.fillAmount = Mathf.Lerp(ringAmmoBar.fillAmount, (float)currentAmmo / clipsize, lerpSpeed);
    }
    public void UpdateAmmoText(int currentAmmo)
    {
        currentAmmoText.text = "" + currentAmmo;
    }
    public void UpdateMagText(int extraAmmo)
    {
        extraAmmoText.text = "" + extraAmmo;
    }
}
