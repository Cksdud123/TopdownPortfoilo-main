using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponClassManager : MonoBehaviour
{
    [SerializeField] public TwoBoneIKConstraint RHandIK;
    [SerializeField] public TwoBoneIKConstraint IHandIK;
    [SerializeField] public Rig rigs;

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
    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();
        IHandIK.data.target = weapon.IHandTarget;
        RHandIK.data.target = weapon.RHandTarget;
        actions.SetWeapon(weapon);
    }
    public void WeaponSwap()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSelect(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponSelect(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapons[currentWeaponIndex].gameObject.SetActive(false);
            anim.SetLayerWeight(1, 0);
            rigs.weight = 0;
        }
    }
    public void WeaponSelect(int weaponIndex)
    {
        rigs.weight = 1;
        anim.SetLayerWeight(1, 1);

        if (currentWeaponIndex == weaponIndex) return;

        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = weaponIndex;

        anim.SetTrigger("Swap");
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
