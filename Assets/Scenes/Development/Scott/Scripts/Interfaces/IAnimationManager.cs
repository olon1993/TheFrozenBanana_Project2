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
    IDLE_LEFT, IDLE_RIGHT, WALK_LEFT, WALK_RIGHT, DASH_START_RIGHT, DASH_STOP_RIGHT, DASH_START_LEFT, DASH_STOP_LEFT
}
