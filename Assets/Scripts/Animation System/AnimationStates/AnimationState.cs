using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationState : MonoBehaviour, IAnimationState
{

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    [SerializeField] protected AnimationClip _animation;
    [SerializeField] protected int _priority;
	[SerializeField] protected int _animationLayer;
	[SerializeField] protected bool _canInterrupt = true;
    [SerializeField] protected bool _isAnimationOverride;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    public virtual int CompareTo(IAnimationState other)
    {
        if (other.Priority > Priority)
        {
            return 1;
        }

        return -1;
    }

    public virtual void UpdateCanInterrupt(bool canInterrupt)
    {
        CanInterrupt = canInterrupt;
    }

    public virtual void OnStateEnter() { }

    public virtual void OnStateExecute() { }

    public virtual void OnStateExit() { }

    public abstract bool ShouldPlay();

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public virtual AnimationClip Animation { get { return _animation; } }

    public virtual bool CanInterrupt 
    { 
        get { return _canInterrupt; } 
        protected set { _canInterrupt = value; } 
    }

    public virtual int Priority { get { return _priority; } }

	public virtual int AnimationLayer { get { return _animationLayer; } }

    public bool IsAnimationOverride 
    {
        get { return _isAnimationOverride; }
        set
        {
            if(_isAnimationOverride != value)
            {
                _isAnimationOverride = value;
            }
        }
    }

}
