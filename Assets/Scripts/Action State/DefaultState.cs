using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DefaultState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.anim.SetBool("Aiming", false);
        actions.currentFov = actions.IdleFov;
    }

    public override void UpdateState(ActionStateManager actions)
    {
        if (Input.GetKey(KeyCode.Mouse1)) actions.SwitchState(actions.AimState);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("������");
            actions.previousState = this;
            actions.SwitchState(actions.Reload);
        }
    }
}
