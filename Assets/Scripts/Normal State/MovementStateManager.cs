using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [HideInInspector] public float currentMoveSpeed; // 현재 이동 속도
    public float walkSpeed = 3, walkBackSpeed = 2; // 걷기속도, 뒤로걷기속도
    public float runSpeed = 7, runBackSpeed = 5; // 뛰기속도, 뒤로뛰기속도


    // 플레이어 이동변수
    private float hInput;
    private float vInput;
    public Vector3 dir;

    CharacterController characterController;
    [HideInInspector] public Animator anim;
    private Camera mainCamera;

    public MovementBaseState previousState;
    public MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        SwitchState(Idle);
    }

    private void Update()
    {
        Move();

        anim.SetFloat("hInput", hInput);
        anim.SetFloat("vInput", vInput);

        currentState.UpdateState(this);
    }
    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    public void Move()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        dir = transform.forward * vInput + transform.right * hInput;
        characterController.Move(dir.normalized * currentMoveSpeed * Time.deltaTime);
    }
}
