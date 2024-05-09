using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class ActionStateManager : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] LayerMask groundMask;

    public ActionBaseState currentState;
    public ActionBaseState previousState;
    [HideInInspector] public Animator anim;

    public DefaultState Default = new DefaultState();
    public AimingState AimState = new AimingState();
    public ReloadState Reload = new ReloadState();

    [HideInInspector] public WeaponClassManager ClassManager;
    [HideInInspector] public WeaponManager currentWeapon;
    [HideInInspector] public WeaponAmmo ammo;
    [HideInInspector] public CinemachineVirtualCamera vCam;

    public float AimingFov = 50f;
    [HideInInspector] public float IdleFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10f;

    public Transform Aimposition;

    public TwoBoneIKConstraint IHandIK;
    public TwoBoneIKConstraint RHandIK;

    void Awake()
    {
        anim = GetComponent<Animator>();
        ClassManager = GetComponent<WeaponClassManager>();
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }
    void Start()
    {
        IdleFov = vCam.m_Lens.FieldOfView;
        SwitchState(Default);
    }
    void Update()
    {
        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmoothSpeed * Time.deltaTime);
        currentState.UpdateState(this);
    }
    private void LateUpdate()
    {
        Aim();
    }
    // Update is called once per frame
    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    IEnumerator WeaponReloaded()
    {
        ammo.Reload();
        yield return null;
        IHandIK.weight = 1;
        RHandIK.weight = 1;
        SwitchState(Default);
    }
    public void SetWeapon(WeaponManager weapon)
    {
        currentWeapon = weapon;
        ammo = weapon.ammo;
    }

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // ���� ���� ���
            var direction = position - transform.position;

            // ���� ���� ����
            direction.y = 0;

            // Transform�� ���콺 ��ġ�� ȸ��
            transform.forward = direction;
        }
    }

    // ���콺 ��ġ�� ���� �� ��ġ ��ȯ
    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // �� ��ġ ������ ���� ����ĳ��Ʈ ����
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
}
