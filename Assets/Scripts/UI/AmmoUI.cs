using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text currentAmmoText;
    public Text extraAmmoText;

    public static AmmoUI instance;

    private void Awake()
    {
        instance = this;
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
