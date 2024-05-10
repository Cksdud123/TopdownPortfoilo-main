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
        actions.IHandIK.weight = Mathf.Lerp(actions.IHandIK.weight, 1, 10 * Time.deltaTime);
        actions.RHandIK.weight = Mathf.Lerp(actions.RHandIK.weight, 1, 10 * Time.deltaTime);

        if (Input.GetKey(KeyCode.Mouse1)) actions.SwitchState(actions.AimState);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("¿Á¿Â¿¸");
            actions.previousState = this;
            actions.SwitchState(actions.Reload);
        }
    }
}
