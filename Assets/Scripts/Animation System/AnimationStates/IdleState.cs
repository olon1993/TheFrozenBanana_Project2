using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AnimationState
{

    public override bool ShouldPlay()
    {
        return true;
    }
}
