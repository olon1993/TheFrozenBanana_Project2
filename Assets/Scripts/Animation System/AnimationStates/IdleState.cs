using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AnimationState
{
    public override void OnStateEnter() { }

    public override void OnStateExecute() { }

    public override void OnStateExit() { }

    public override bool ShouldPlay()
    {
        return true;
    }
}
