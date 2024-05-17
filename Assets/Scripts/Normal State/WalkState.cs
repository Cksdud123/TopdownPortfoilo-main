using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    private float sprint_Rate = 5f;
    public override void EnterState(MovementStateManager movement) => movement.anim.SetBool("Walking", true);

    public override void UpdateState(MovementStateManager movement)
    {
        movement.currentMoveSpeed = movement.walkSpeed;

        movement.health.AddStamina(sprint_Rate * Time.deltaTime);

        if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
        else if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);

    }
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
