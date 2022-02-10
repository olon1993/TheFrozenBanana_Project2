using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAnimationState
{
    IDLE, WALK, DASH, ATTACK, JUMP_STRAIGHT, JUMP_SIDE, FALL_STRAIGHT, FALL_SIDE, WALLSLIDE
}

public class CharacterAnimationManager : MonoBehaviour
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private Animator _animator;
    private CharacterAnimationState _currentAnimationState;
    private AltCombatant _combatant;
    private AltLocomotion2D _locomotion2D;

    // Timer
    [SerializeField] float fallDelayTime = 0.3f;
    float fallDelayTimer = 0f;

    [SerializeField] List<CharacterAnimationState> ImmutableAnimationStates;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    void Start()
    {
        GetDependencies();
        
        RequestStateChange(CharacterAnimationState.IDLE);
    }

    void GetDependencies()
    {
        _animator = transform.Find("Graphics").GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }

        _combatant = GetComponent<AltCombatant>();

        if (_combatant == null)
        {
            Debug.LogError("Combatant not found on " + gameObject.name);
        }

        _locomotion2D = GetComponent<AltLocomotion2D>();

        if (_locomotion2D == null)
        {
            Debug.LogError("Locomotion2D not found on " + gameObject.name);
        }
    }

    public void OnAttack()
    {
        RequestStateChange(CharacterAnimationState.ATTACK);
    }

    public void LocomotionChecks()
    {
        if (_combatant.IsAttacking) return;

        Vector2 _velocity = _locomotion2D.Velocity;

        if(!_locomotion2D.IsGrounded)
        {
            if (_locomotion2D.IsWallSliding)
            {
                RequestStateChange(CharacterAnimationState.WALLSLIDE);
                return;
            }
            if (_velocity.y > 0)
            {
                if (Mathf.Abs(_velocity.x) > Mathf.Epsilon)
                    RequestStateChange(CharacterAnimationState.JUMP_SIDE);
                else
                    RequestStateChange(CharacterAnimationState.JUMP_STRAIGHT);
                return;
            }
            if (_velocity.y < 0)
            {
                if (fallDelayTimer > fallDelayTime)
                {
                    fallDelayTimer = 0f;
                    if (Mathf.Abs(_velocity.x) > Mathf.Epsilon)
                        RequestStateChange(CharacterAnimationState.FALL_SIDE);
                    else
                        RequestStateChange(CharacterAnimationState.FALL_STRAIGHT);
                    return;
                }
                else
                {
                    fallDelayTimer += Time.deltaTime;
                }
            }
        }
        if (Mathf.Abs(_velocity.x) <= Mathf.Epsilon && Mathf.Abs(_velocity.y) <= Mathf.Epsilon)
        {

            RequestStateChange(CharacterAnimationState.IDLE);
            return;
        }

        if (Mathf.Abs(_velocity.x) > Mathf.Epsilon && Mathf.Abs(_velocity.y) <= Mathf.Epsilon)
        {
            if (_locomotion2D.IsDashing)
            {
                RequestStateChange(CharacterAnimationState.DASH);
            }
            else
            {
                RequestStateChange(CharacterAnimationState.WALK);
            }
        }
    }

    // Call this method whenever the animation state must be updated
    private void RequestStateChange(CharacterAnimationState newState)
    {
        // Do no further processing if the requested state is already active
        // This is important for RequestStateChange being used in Update or Coroutines
        if (_currentAnimationState == newState)
        {
            return;
        }

        // Do no further processing if the animation cannot be interrupted
        if (!CanInterrupt())
        {
            return;
        }

        _currentAnimationState = newState;

        // New animations need to be added to this switch statement
        switch (newState)
        {
            case CharacterAnimationState.IDLE:
                _animator.SetTrigger("idle");
                break;
            case CharacterAnimationState.WALK:
                _animator.SetTrigger("walk");
                break;
            case CharacterAnimationState.DASH:
                _animator.SetTrigger("dash");
                break;
            case CharacterAnimationState.ATTACK:
                _animator.SetTrigger("punch");
                break;
            case CharacterAnimationState.JUMP_STRAIGHT:
                _animator.SetTrigger("jumpStraight");
                break;
            case CharacterAnimationState.JUMP_SIDE:
                _animator.SetTrigger("jumpSide");
                break;
            case CharacterAnimationState.FALL_STRAIGHT:
                _animator.SetTrigger("fallStraight");
                break;
            case CharacterAnimationState.FALL_SIDE:
                _animator.SetTrigger("fallSide");
                break;
            case CharacterAnimationState.WALLSLIDE:
                _animator.SetTrigger("wallSlide");
                break;
        }

        Debug.Log("Current animation state: " + _currentAnimationState);
    }

    private bool CanInterrupt()
    {
        if (ImmutableAnimationStates.Contains(_currentAnimationState) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            return false;
        }

        return true;
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public CharacterAnimationState GetCurrentAnimationState()
    {
        return _currentAnimationState;
    }
}
