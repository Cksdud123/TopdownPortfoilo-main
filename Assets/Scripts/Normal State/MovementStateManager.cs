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

    [SerializeField] float groundYOffset; // 땅에서의 높이 오프셋
    [SerializeField] LayerMask groundMask; // 땅을 확인하기 위한 레이어 마스크
    Vector3 spherePos; // 구의 위치

    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;
    [HideInInspector] public bool jumped; // 점프 여부
    Vector3 velocity; // 속도 벡터

    [HideInInspector] public Health health;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
        SwitchState(Idle);
    }

    private void Update()
    {
        Move();
        Gravity();

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
    public bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, characterController.radius - 0.05f, groundMask)) return true;
        return false;
    }
    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        characterController.Move(velocity * Time.deltaTime);
    }
}
