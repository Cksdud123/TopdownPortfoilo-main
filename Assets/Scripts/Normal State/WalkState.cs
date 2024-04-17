using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(MovementStateManager movement) => movement.anim.SetBool("Walking", true);

    public override void UpdateState(MovementStateManager movement)
    {
        movement.currentMoveSpeed = movement.walkSpeed;

        if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.Idle);
        else if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.Run);

    }
    void ExitState(MovementStateManager movement, MovementBaseState state)
    {
        movement.anim.SetBool("Walking", false);
        movement.SwitchState(state);
    }
}
