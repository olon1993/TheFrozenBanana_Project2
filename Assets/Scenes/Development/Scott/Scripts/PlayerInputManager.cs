using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private ILocomotion _locomotion;
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
        
        //_animationStateManager = transform.GetComponent<IAnimationManager>();

    }

    // Update is called once per frame
    void Update()
    {
        // Locomotion
        _locomotion.HorizontalMovement = Input.GetAxisRaw("Horizontal");
        _locomotion.VerticalMovement = Input.GetAxisRaw("Vertical");
        _locomotion.IsJumping = Input.GetButtonDown("Jump");
        _locomotion.IsJumpCancelled = Input.GetButtonUp("Jump");
        _locomotion.IsDashing = Input.GetButtonDown("Fire3");

        // Combatant
        _combatant.HorizontalFacingDirection = (int)Input.GetAxisRaw("Horizontal");
        _combatant.IsAttacking = Input.GetButtonDown("Fire1");

        if (_showDebugLog)
        {
            Debug.Log("Horizontal: " + _locomotion.HorizontalMovement);
            Debug.Log("Vertical: " + _locomotion.VerticalMovement);
            Debug.Log("IsJumping: " + _locomotion.IsJumping);
            Debug.Log("IsJumpCancelled: " + _locomotion.IsJumpCancelled);
            Debug.Log("IsDashing: " + _locomotion.IsDashing);
        }
    }
}
