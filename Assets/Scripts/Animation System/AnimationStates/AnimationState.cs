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
    [SerializeField] protected bool _canInterrupt = true;

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

    public abstract void OnStateEnter();

    public abstract void OnStateExecute();

    public abstract void OnStateExit();

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
}
