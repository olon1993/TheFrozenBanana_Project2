using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\
    // Movement
    public bool IsMovementEnabled = true;

    // Dependencies
    protected AltLocomotion2D _locomotion2D;
    protected AltCombatant _combatant;
    protected CharacterAnimationManager _characterAnimation;

    // Inputs
    protected float horizontal = 0f;
    protected float vertical = 0f;
    protected bool jump, cancelJump;
    protected bool dash;
    protected bool attack;

    protected virtual void Awake()
    {
        FindDependencies();
    }

    void FindDependencies()
    {
        _locomotion2D = GetComponent<AltLocomotion2D>();

        if(_locomotion2D == null)
        {
            Debug.LogError("No Locomotion component found on " + gameObject.name);
        }
        _combatant = GetComponent<AltCombatant>();

        if (_combatant == null)
        {
            Debug.Log("No Combatant component found on " + gameObject.name);
        }
        _characterAnimation = GetComponent<CharacterAnimationManager>();

        if (_characterAnimation == null)
        {
            Debug.Log("No CharacterAnimation component found on " + gameObject.name);
        }
    }

    protected virtual void Update()
    {
        DetermineInput();

        DetermineAttack();

        Movement();

        DebugInfo();
    }

    protected virtual void DetermineInput() {  }

    protected virtual void DetermineAttack()
    {
        if (_combatant != null && attack)
        {
            _combatant.OnAttack();

            if (_characterAnimation != null)
            {
                _characterAnimation.OnAttack();
            }
        }
    }

    protected virtual void Movement()
    {
        if(IsMovementEnabled)
            _locomotion2D.HandleMovement(new Vector2(horizontal, vertical), jump, cancelJump, dash);
    }

    protected virtual void DebugInfo()
    {
        if (!_showDebugLog) return;

        Debug.Log("Horizontal: " + horizontal);
        Debug.Log("Vertical: " + vertical);
        Debug.Log("Jumping: " + jump);
        Debug.Log("IsJumpCancelled: " + cancelJump);
        Debug.Log("IsDashing: " + dash);
        Debug.Log("IsAttacking: " + attack);
    }
}
