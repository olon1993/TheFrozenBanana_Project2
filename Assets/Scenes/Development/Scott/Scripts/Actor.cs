using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    protected ILocomotion _locomotion;
    protected IAnimationManager _animationManager;
    protected ICombatant _combatant;


    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _locomotion = transform.GetComponent<ILocomotion>();
        if (_locomotion == null)
        {
            Debug.LogError("ILocomotion not found on " + name);
        }

        _combatant = transform.GetComponent<ICombatant>();
        if (_combatant == null)
        {
            Debug.LogError("ICombatant not found on " + name);
        }

        _animationManager = transform.GetComponent<IAnimationManager>();
        if (_animationManager == null)
        {
            Debug.LogError("IAnimationManager not found on " + name);
        }
    }

    protected virtual void CalculateAnimationState()
    {
        // Attack
        if (_combatant.IsAttacking)
        {
            if (_locomotion.HorizontalLook > 0)
            {
                _animationManager.RequestStateChange(AnimationState.ATTACK_RIGHT);
            }
            else
            {
                _animationManager.RequestStateChange(AnimationState.ATTACK_LEFT);
            }
        }
        // Dash
        else if (_locomotion.IsDashCancelled)
        {
            if (_animationManager.GetCurrentAnimationState() == AnimationState.DASH_START_RIGHT)
            {
                _animationManager.RequestStateChange(AnimationState.DASH_STOP_RIGHT);
            }
            else if (_animationManager.GetCurrentAnimationState() == AnimationState.DASH_START_LEFT)
            {
                _animationManager.RequestStateChange(AnimationState.DASH_STOP_LEFT);
            }
        }
        else if (Mathf.Abs(_locomotion.Velocity.y) > 0.25f)
        {
            if (_locomotion.HorizontalLook > 0)
            {
                if (_locomotion.IsRightCollision)
                {
                    _animationManager.RequestStateChange(AnimationState.WALL_SLIDE_RIGHT);
                }
                else if (_locomotion.Velocity.y > 0)
                {
                    _animationManager.RequestStateChange(AnimationState.JUMP_RIGHT);
                }
                else
                {
                    _animationManager.RequestStateChange(AnimationState.FALL_RIGHT);
                }
            }
            else
            {
                if (_locomotion.IsLeftCollision)
                {
                    _animationManager.RequestStateChange(AnimationState.WALL_SLIDE_LEFT);
                }
                else if (_locomotion.Velocity.y > 0)
                {
                    _animationManager.RequestStateChange(AnimationState.JUMP_LEFT);
                }
                else
                {
                    _animationManager.RequestStateChange(AnimationState.FALL_LEFT);
                }
            }
        }
        else
        {
            if (_locomotion.HorizontalMovement != 0)
            {
                if (_locomotion.HorizontalMovement > 0)
                {
                    if (_locomotion.IsDashing && _locomotion.IsGrounded)
                    {
                        _animationManager.RequestStateChange(AnimationState.DASH_START_RIGHT);
                    }
                    else
                    {
                        _animationManager.RequestStateChange(AnimationState.WALK_RIGHT);
                    }
                }
                else
                {
                    if (_locomotion.IsDashing && _locomotion.IsGrounded)
                    {
                        _animationManager.RequestStateChange(AnimationState.DASH_START_LEFT);
                    }
                    else
                    {
                        _animationManager.RequestStateChange(AnimationState.WALK_LEFT);
                    }
                }
            }
            else
            {
                if (_locomotion.HorizontalLook > 0)
                {
                    _animationManager.RequestStateChange(AnimationState.IDLE_RIGHT);
                }
                else
                {
                    _animationManager.RequestStateChange(AnimationState.IDLE_LEFT);
                }
            }
        }
    }
}
