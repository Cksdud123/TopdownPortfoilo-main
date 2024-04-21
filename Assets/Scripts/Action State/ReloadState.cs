using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ReloadState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.RHandIK.weight = 0;
        actions.IHandIK.weight = 0;
        actions.anim.SetTrigger("Reloading");
    }

    public override void UpdateState(ActionStateManager actions)
    {
        /*if (actions.previousState == actions.Default)
        {
            actions.SwitchState(actions.Default);
        }
        else if (actions.previousState == actions.AimState)
        {
            actions.SwitchState(actions.AimState);
        }*/
    }
}
