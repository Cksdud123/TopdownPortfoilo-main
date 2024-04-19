using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponChange : MonoBehaviour
{
    [SerializeField] public Rig[] rigs;
    [HideInInspector] public Animator anim;

    public WeaponManager[] weapons;
    ActionStateManager actions;
    int currentWeaponIndex;
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
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }
    private void Update()
    {
        WeaponSwap();
    }
    public void WeaponSwap()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSelect(0);
            // ���Ⱑ ���� ������
            //weapons[currentWeaponIndex].gameObject.SetActive(true);
            //anim.SetTrigger("Swap");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponSelect(1);
        }
    }
    public void WeaponSelect(int weaponIndex)
    {
        //rig.weight = 1;
        //anim.SetLayerWeight(1, 1);

        if (currentWeaponIndex == weaponIndex) return;

        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = weaponIndex;

        anim.SetTrigger("Swap");
    }
    /*public void SetActiveRig(int rigIndex)
    {
        // ��� ������ ��Ȱ��ȭ�մϴ�.
        foreach (var rig in rigs)
        {
            rig.weight = 0f;
        }

        // ������ ������ Ȱ��ȭ�մϴ�.
        if (rigIndex >= 0 && rigIndex < rigs.Length)
        {
            rigs[rigIndex].weight = 1f;
        }
    }*/
}
