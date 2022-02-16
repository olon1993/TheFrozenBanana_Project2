using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IAnimationManager
    {
        void RequestStateChange(AnimationState newState);
        AnimationState GetCurrentAnimationState();
    }

    public enum AnimationState
    {
        IDLE_LEFT, IDLE_RIGHT,
        WALK_LEFT, WALK_RIGHT,
        DASH_START_LEFT, DASH_START_RIGHT,
        DASH_STOP_LEFT, DASH_STOP_RIGHT,
        ATTACK_LEFT, ATTACK_RIGHT,
        JUMP_LEFT, JUMP_RIGHT,
        FALL_LEFT, FALL_RIGHT,
        WALL_SLIDE_LEFT, WALL_SLIDE_RIGHT
    }
}
