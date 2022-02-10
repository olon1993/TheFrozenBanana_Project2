using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PunchAnimation : MonoBehaviour
{
    public UnityEvent OnAttackAnimationFinished;
    public void AnimationFinished()
    {
        Debug.Log("Punch animation finished");
        OnAttackAnimationFinished.Invoke();
        /*MyCombatant[] combatants = gameObject.GetComponentsInParent<MyCombatant>();
        combatants[0].AttackAnimationFinished();*/
    }
}
