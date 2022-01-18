using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationManager 
{
    void RequestStateChange(AnimationState newState);
    AnimationState GetCurrentAnimationState();
}

public enum AnimationState
{
    IDLE, WALK, RUN, JUMP, AIRBORNE, LAND, ATTACK_01, ATTACK_02, ATTACK_03
}
