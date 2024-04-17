using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ActionStateManager : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;
    public ActionBaseState currentState;
    public ActionBaseState previousState;
    [HideInInspector] public Animator anim;

    public DefaultState Default = new DefaultState();
    public AimingState AimState = new AimingState();
    public ReloadState Reload = new ReloadState();

    [HideInInspector] public WeaponManager WeaponManager;
    [HideInInspector] public WeaponAmmo ammo;
    [HideInInspector] public CinemachineVirtualCamera vCam;

    public float AimingFov = 50f;
    [HideInInspector] public float IdleFov;
    [HideInInspector] public float currentFov;
    public float fovSmoothSpeed = 10f;
    //public TwoBoneIKConstraint lHandIK;
    //public TwoBoneIKConstraint RHandIK;

    void Awake()
    {
        anim = GetComponent<Animator>();
        ammo = GetComponentInChildren<WeaponAmmo>();
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
        AimCamera();
    }
    // Update is called once per frame
    public void SwitchState(ActionBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    public void AimCamera()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;

            direction.y = 0;

            transform.forward = direction;
        }
    }
    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {

            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
    private void OnDrawGizmos()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;
            direction.y = 0;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}
