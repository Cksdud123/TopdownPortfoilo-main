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
    public void SetCurrentWeapon(WeaponManager weapon)
    {
        if (actions == null) actions = GetComponent<ActionStateManager>();
        IHandIK.data.target = weapon.IHandTarget;
        RHandIK.data.target = weapon.RHandTarget;
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
            StartCoroutine(WeaponSelect(0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(WeaponSelect(1));
        }
    }

    IEnumerator WeaponSelect(int weaponIndex)
    {
        rigs.weight = 1;
        anim.SetLayerWeight(1, 1);

        if (currentWeaponIndex == weaponIndex) yield break;

        anim.SetTrigger("Swap");

        yield return new WaitForSeconds(1f);
        // 현재 무기를 비활성화 한다
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        // 입력으로 들어온 무기를 활성화 한다.
        weapons[weaponIndex].gameObject.SetActive(true);

        // 입력으로 들어온 무기를 현재 무기로 변경한다.
        currentWeaponIndex = weaponIndex;
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
