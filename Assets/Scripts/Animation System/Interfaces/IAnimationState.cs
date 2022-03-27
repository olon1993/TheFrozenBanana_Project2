using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationState : IComparable<IAnimationState>
{
    AnimationClip Animation { get; }
    bool ShouldPlay();
    void OnStateEnter();
    void OnStateExecute();
    void OnStateExit();
    void UpdateCanInterrupt(bool canInterrupt);
    bool CanInterrupt { get; }
    int Priority { get; }
	int AnimationLayer { get; }
    bool IsAnimationOverride { get;  set; }
}
