using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    private float sprint_Rate = 10f;

    public override void EnterState(MovementStateManager movement) { }

    public override void UpdateState(MovementStateManager movement)
    {
        movement.health.AddStamina(sprint_Rate * Time.deltaTime);

        if (movement.dir.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.Run);
            else movement.SwitchState(movement.Walk);
        }
    }
}
