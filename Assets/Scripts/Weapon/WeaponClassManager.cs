using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] public TwoBoneIKConstraint RHandIK;
    [SerializeField] public TwoBoneIKConstraint IHandIK;
    [SerializeField] public RigBuilder rigBuilder;

    [SerializeField] private List<Rig> rigs = new List<Rig>();

    [HideInInspector] public Animator anim;

    public Transform recoilFollowPos;

    public WeaponManager[] weapons;
    ActionStateManager actions;

    [HideInInspector] public int currentWeaponIndex;
    int currentLayerIndex;
    int currentRigIndex;
    // Start is called before the first frame update

    private void Awake()
    {
        anim = GetComponent<Animator>();
        actions = GetComponent<ActionStateManager>();
    }
    void Start()
    {
        //anim.SetLayerWeight(1, 0);
        currentWeaponIndex = 0;
        currentRigIndex = 0;
        currentLayerIndex = 1;

        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(3, 0);
        weapons[currentWeaponIndex].gameObject.SetActive(true);
        rigBuilder.layers[currentRigIndex].active = true;
    }
    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();

        // 현재 무기의 탄약 정보를 UI에 업데이트
        AmmoUI.instance.UpdateAmmoText(weapon.ammo.currentAmmo);
        AmmoUI.instance.UpdateMagText(weapon.ammo.extraAmmo);
        //AmmoUI.instance.AmmoBarFilter(weapon.ammo.currentAmmo, weapon.ammo.clipSize);

        IHandIK.data.target = weapon.IHandTarget;
        RHandIK.data.target = weapon. RHandTarget;

        rigBuilder.Build();

        actions.SetWeapon(weapon);
    }
    private void Update()
    {
        WeaponSwap();
    }
    public void WeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(WeaponSelect(0,1,0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(WeaponSelect(1, 2, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(WeaponSelect(2, 3, 2));
        }
    }

    IEnumerator WeaponSelect(int weaponIndex,int LayerWeight,int rigWeight)
    {
        AmmoUI.instance.ChangeICon(weaponIndex);

        if (currentWeaponIndex == weaponIndex) yield break;

        anim.SetTrigger("Swap");

        yield return new WaitForSeconds(1f);
        // 현재 무기를 비활성화 한다
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        // 입력으로 들어온 무기를 활성화 한다.
        weapons[weaponIndex].gameObject.SetActive(true);

        // 입력으로 들어온 무기를 현재 무기로 변경한다.
        currentWeaponIndex = weaponIndex;

        LayerSelect(weaponIndex, LayerWeight, rigWeight);
    }
    public void LayerSelect(int weaponSelect, int LayerSelect, int rigWeight)
    {
        rigs[currentRigIndex].weight = 0;

        rigBuilder.layers[currentRigIndex].active = false;

        rigBuilder.layers[rigWeight].active = true;

        rigs[rigWeight].weight = 1;

        currentRigIndex = rigWeight;

        // 현재 가중치를 비활성화 한다
        anim.SetLayerWeight(currentLayerIndex, 0);
        // 입력으로 들어온 가중치를 활성화 한다
        anim.SetLayerWeight(LayerSelect, 1);
        // 입력으로 들어온 가중치를 현재 가중치로 설정한다.
        currentLayerIndex = LayerSelect;
    }
    IEnumerator WeaponPutAway()
    {
        yield return null;
        IHandIK.weight = 0;
        RHandIK.weight = 0;
    }
    IEnumerator WeaponPulledOut()
    {
        yield return null;
        IHandIK.weight = 1;
        RHandIK.weight = 1;
        actions.SwitchState(actions.Default);
    }
}
