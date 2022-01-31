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
    IDLE_LEFT, IDLE_RIGHT, WALK_LEFT, WALK_RIGHT
}
