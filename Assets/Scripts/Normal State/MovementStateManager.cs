using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    [HideInInspector] public float currentMoveSpeed; // ���� �̵� �ӵ�
    public float walkSpeed = 3, walkBackSpeed = 2; // �ȱ�ӵ�, �ڷΰȱ�ӵ�
    public float runSpeed = 7, runBackSpeed = 5; // �ٱ�ӵ�, �ڷζٱ�ӵ�


    // �÷��̾� �̵�����
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
