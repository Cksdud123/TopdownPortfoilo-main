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
    public float rotationSpeed = 1f;

    public TwoBoneIKConstraint IHandIK;
    public TwoBoneIKConstraint RHandIK;

    AudioSource audioSource;

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
    public void WeaponReloaded()
    {
        ammo.Reload();
        SwitchState(Default);
    }
    public void SetWeapon(WeaponManager weapon)
    {
        currentWeapon = weapon;
        audioSource = weapon.audioSource;
        ammo = weapon.ammo;
    }
    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // 조준 방향 계산
            var direction = position - transform.position;

            // 높이 차이 무시
            direction.y = 0;

            // Transform을 마우스 위치로 회전
            transform.forward = direction;
        }
    }

    // 마우스 위치에 따른 땅 위치 반환
    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 땅 위치 감지를 위한 레이캐스트 실행
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
    public void MagOut()
    {
        audioSource.PlayOneShot(ammo.magOutSound);
    }

    public void MagIn()
    {
        audioSource.PlayOneShot(ammo.magInSound);
    }

    public void ReleaseSlide()
    {
        audioSource.PlayOneShot(ammo.releaseSlideSound);
    }
}
