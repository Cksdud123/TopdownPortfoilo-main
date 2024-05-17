using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    //public Image AmmoBar;
    public Image[] ICon;

    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI extraAmmoText;

    public static AmmoUI instance;

    private int IconCount;
    private int currentIConIndex;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        ICon[currentIConIndex].gameObject.SetActive(true);
    }
    public void ChangeICon(int IconCount)
    {
        ICon[currentIConIndex].gameObject.SetActive(false);

        ICon[IconCount].gameObject.SetActive(true);

        currentIConIndex = IconCount;
    }
    public void UpdateAmmoText(int currentAmmo)
    {
        currentAmmoText.text = "" + currentAmmo;
    }
    public void UpdateMagText(int extraAmmo)
    {
        extraAmmoText.text = "" + extraAmmo;
    }

    /*public void AmmoBarFilter(int currentAmmo, int clipsize)
    {
        AmmoBar.fillAmount = Mathf.Lerp(AmmoBar.fillAmount, (float)currentAmmo / clipsize, 1);
    }*/
}
