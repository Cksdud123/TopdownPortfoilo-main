using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.anim.SetBool("Aiming", true);
        actions.currentFov = actions.AimingFov;
    }
    public override void UpdateState(ActionStateManager actions)
    {
        if (Input.GetKeyUp(KeyCode.Mouse1)) actions.SwitchState(actions.Default);

        if (Input.GetKeyDown(KeyCode.R))
        {
            actions.previousState = this;
            actions.SwitchState(actions.Reload);
        }
    }
}
