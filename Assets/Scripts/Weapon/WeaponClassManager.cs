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

        // ���� ������ ź�� ������ UI�� ������Ʈ
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
        // ���� ���⸦ ��Ȱ��ȭ �Ѵ�
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        // �Է����� ���� ���⸦ Ȱ��ȭ �Ѵ�.
        weapons[weaponIndex].gameObject.SetActive(true);

        // �Է����� ���� ���⸦ ���� ����� �����Ѵ�.
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

        // ���� ����ġ�� ��Ȱ��ȭ �Ѵ�
        anim.SetLayerWeight(currentLayerIndex, 0);
        // �Է����� ���� ����ġ�� Ȱ��ȭ �Ѵ�
        anim.SetLayerWeight(LayerSelect, 1);
        // �Է����� ���� ����ġ�� ���� ����ġ�� �����Ѵ�.
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
