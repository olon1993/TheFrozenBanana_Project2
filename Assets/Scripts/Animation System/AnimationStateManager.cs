using System.Collections.Generic;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour, IAnimationManager
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private Animator _animator;
    private AnimationState _currentAnimationState;

    [SerializeField] List<AnimationState> ImmutableAnimationStates;

    private const string IDLE_RIGHT_NAME = "Idle_right";
    private const string IDLE_LEFT_NAME = "Idle_left";
    private const string WALK_RIGHT_NAME = "Run_right";
    private const string WALK_LEFT_NAME = "Run_left";
    private const string DASH_START_RIGHT_NAME = "Dash_start_right";
    private const string DASH_STOP_RIGHT_NAME = "Dash_stop_right";
    private const string DASH_START_LEFT_NAME = "Dash_start_left";
    private const string DASH_STOP_LEFT_NAME = "Dash_stop_left";

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.Find("Graphics").GetComponent<Animator>();

        if(_animator == null)
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }

        RequestStateChange(AnimationState.IDLE_RIGHT);
    }

    // Call this method externally whenever the animation state must be updated
    public void RequestStateChange(AnimationState newState)
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
            case AnimationState.IDLE_RIGHT:
                _animator.Play(IDLE_RIGHT_NAME);
                break;
            case AnimationState.IDLE_LEFT:
                _animator.Play(IDLE_LEFT_NAME);
                break;
            case AnimationState.WALK_RIGHT:
                _animator.Play(WALK_RIGHT_NAME);
                break;
            case AnimationState.WALK_LEFT:
                _animator.Play(WALK_LEFT_NAME);
                break;
            case AnimationState.DASH_START_RIGHT:
                _animator.Play(DASH_START_RIGHT_NAME);
                break;
            case AnimationState.DASH_START_LEFT:
                _animator.Play(DASH_START_LEFT_NAME);
                break;
            case AnimationState.DASH_STOP_RIGHT:
                _animator.Play(DASH_STOP_RIGHT_NAME);
                break;
            case AnimationState.DASH_STOP_LEFT:
                _animator.Play(DASH_STOP_LEFT_NAME);
                break;
        }
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

    public AnimationState GetCurrentAnimationState()
    {
        return _currentAnimationState;
    }

}
