using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private ILocomotion _locomotion;
    private IAnimationManager _animationManager;
    private ICombatant _combatant;
    
    // Movement
    public bool IsMovementEnabled = true;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    void Awake()
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

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Combatant
        _combatant.HorizontalFacingDirection = (int)horizontal;
        _combatant.IsAttacking = Input.GetButtonDown("Fire1");

        if (_combatant.IsAttacking == false)
        {
            // Locomotion
            _locomotion.HorizontalMovement = horizontal;
            _locomotion.VerticalMovement = vertical;
            _locomotion.IsJumping = Input.GetButtonDown("Jump");
            _locomotion.IsJumpCancelled = Input.GetButtonUp("Jump");
            _locomotion.IsDashing = _locomotion.IsJumping ? false : Input.GetButton("Fire3");
            _locomotion.IsDashCancelled = _locomotion.IsDashing && Mathf.Abs(horizontal) <= Mathf.Epsilon;
        }

        CalculateAnimationState();

        if (_showDebugLog)
        {
            Debug.Log("Horizontal: " + _locomotion.HorizontalMovement);
            Debug.Log("Vertical: " + _locomotion.VerticalMovement);
            Debug.Log("Horizontal Look: " + _locomotion.HorizontalLook);
            Debug.Log("Right Collision: " + _locomotion.IsRightCollision);
            Debug.Log("IsJumping: " + _locomotion.IsJumping);
            Debug.Log("IsJumpCancelled: " + _locomotion.IsJumpCancelled);
            Debug.Log("IsDashing: " + _locomotion.IsDashing);

            Debug.Log("IsAttacking: " + _combatant.IsAttacking);
        }
    }

    private void CalculateAnimationState()
    {
        // Animation
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
