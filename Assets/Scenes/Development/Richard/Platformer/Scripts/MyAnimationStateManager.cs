using System;
using System.Collections.Generic;
using UnityEngine;

public enum MyAnimationState
{
    IDLE, WALK, DASH, ATTACK, JUMP_STRAIGHT, FALL_STRAIGHT, WALLSLIDE_LEFT, WALLSLIDE_RIGHT
}

public class MyAnimationStateManager : MonoBehaviour
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private Animator _animator;
    private MyAnimationState _currentAnimationState;
    private MyCombatant _combatant;
    private MyLocomotion2D _locomotion2D;

    [SerializeField] List<MyAnimationState> ImmutableAnimationStates;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.Find("Graphics").GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }

        _combatant = GetComponent<MyCombatant>();

        if (_combatant == null)
        {
            Debug.LogError("Combatant not found on " + gameObject.name);
        }

        _locomotion2D = GetComponent<MyLocomotion2D>();

        if (_locomotion2D == null)
        {
            Debug.LogError("Locomotion2D not found on " + gameObject.name);
        }

        RequestStateChange(MyAnimationState.IDLE);
    }

    void Update()
    {
        if (CombatantChecks()) return;
        LocomotionChecks();
    }

    void LocomotionChecks()
    {
        Vector2 _velocity = _locomotion2D.Velocity;

        if(_locomotion2D.IsWallSliding)
        {
            if(_locomotion2D.WallDirectionX < 0)
            {
                RequestStateChange(MyAnimationState.WALLSLIDE_RIGHT);
                return;
            }
            if(_locomotion2D.WallDirectionX > 0)
            {
                RequestStateChange(MyAnimationState.WALLSLIDE_LEFT);
                return;
            }
        }

        if(!_locomotion2D.IsGrounded)
        {
            if(_velocity.y > 0)
            {
                RequestStateChange(MyAnimationState.JUMP_STRAIGHT);
                return;
            }
            if (_velocity.y < 0)
            {
                RequestStateChange(MyAnimationState.FALL_STRAIGHT);
                return;
            }
        }
        if (Mathf.Abs(_velocity.x) <= Mathf.Epsilon && Mathf.Abs(_velocity.y) <= Mathf.Epsilon)
        {

            RequestStateChange(MyAnimationState.IDLE);
            return;
        }

        if (Mathf.Abs(_velocity.x) > Mathf.Epsilon && _locomotion2D.IsGrounded)
        {
            if (_locomotion2D.IsDashing)
            {
                RequestStateChange(MyAnimationState.DASH);
            }
            else
            {
                RequestStateChange(MyAnimationState.WALK);
            }
        }
    }

    bool CombatantChecks()
    {
        if (_combatant.IsAttacking)
        {
            RequestStateChange(MyAnimationState.ATTACK);
            return true;
        }
        else return false;
    }

    // Call this method externally whenever the animation state must be updated
    public void RequestStateChange(MyAnimationState newState)
    {
        // Do no further processing if the requested state is already active
        // This is important for RequestStateChange being used in Update or Coroutines
        if (_currentAnimationState == newState)
        {
            return;
        }

        // Do no further processing if the animation cannot be interrupted
        if (CanInterrupt() == false)
        {
            return;
        }

        _currentAnimationState = newState;

        // New animations need to be added to this switch statement
        switch (newState)
        {
            case MyAnimationState.IDLE:
                _animator.SetTrigger("idle");
                break;
            case MyAnimationState.WALK:
                _animator.SetTrigger("walk");
                break;
            case MyAnimationState.DASH:
                _animator.SetTrigger("dash");
                break;
            case MyAnimationState.ATTACK:
                _animator.SetTrigger("punch");
                break;
            case MyAnimationState.JUMP_STRAIGHT:
                _animator.SetTrigger("jumpStraight");
                break;
            case MyAnimationState.FALL_STRAIGHT:
                _animator.SetTrigger("fallStraight");
                break;
            case MyAnimationState.WALLSLIDE_RIGHT:
                _animator.SetTrigger("wallSlideRight");
                break;
            case MyAnimationState.WALLSLIDE_LEFT:
                _animator.SetTrigger("wallSlideLeft");
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

    public MyAnimationState GetCurrentAnimationState()
    {
        return _currentAnimationState;
    }
}
