using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingState : ActionBaseState
{
    public float maxYPosition = 2f;
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
            Debug.Log("������");
            actions.previousState = this;
            actions.SwitchState(actions.Reload);
        }
    }
}
