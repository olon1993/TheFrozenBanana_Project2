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

        // Locomotion
        _locomotion.HorizontalMovement = horizontal;
        _locomotion.VerticalMovement = vertical;
        _locomotion.IsJumping = Input.GetButtonDown("Jump");
        _locomotion.IsJumpCancelled = Input.GetButtonUp("Jump");
        _locomotion.IsDashing = Input.GetButtonDown("Fire3");

        // Animation
        if (horizontal != 0)
        {
            if(horizontal > 0)
            {
                _animationManager.RequestStateChange(AnimationState.IDLE_RIGHT);
            }
            else
            {
                _animationManager.RequestStateChange(AnimationState.IDLE_LEFT);
            }
        }

        // Combatant
        _combatant.HorizontalFacingDirection = (int)horizontal;
        _combatant.IsAttacking = Input.GetButtonDown("Fire1");

        if (_showDebugLog)
        {
            Debug.Log("Horizontal: " + _locomotion.HorizontalMovement);
            Debug.Log("Vertical: " + _locomotion.VerticalMovement);
            Debug.Log("IsJumping: " + _locomotion.IsJumping);
            Debug.Log("IsJumpCancelled: " + _locomotion.IsJumpCancelled);
            Debug.Log("IsDashing: " + _locomotion.IsDashing);

            Debug.Log("IsAttacking: " + _combatant.IsAttacking);
        }
    }
}
