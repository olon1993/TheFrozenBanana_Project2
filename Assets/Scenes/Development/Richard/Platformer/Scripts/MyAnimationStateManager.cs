using System.Collections.Generic;
using UnityEngine;

public enum MyAnimationState
{
    IDLE, WALK, DASH
}

public class MyAnimationStateManager : MonoBehaviour
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private Animator _animator;
    private MyAnimationState _currentAnimationState;

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

        RequestStateChange(MyAnimationState.IDLE);
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
        }

        Debug.Log("Current animation state: " + _currentAnimationState);
    }

    private bool CanInterrupt()
    {
        if (ImmutableAnimationStates.Contains(_currentAnimationState))
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
