using UnityEngine;

public class MyPlayerInputManager : MonoBehaviour, IInputManager
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private ILocomotion _locomotion;
    private IAnimationManager _animationManager;
    private ICombatant _combatant;

    // Inputs
    float horizontal;
    float vertical;
    bool jump, cancelJump;
    bool dash;

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
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        cancelJump = Input.GetButtonUp("Jump");
        dash = Input.GetButton("Fire3");

        // Combatant
        _combatant.HorizontalFacingDirection = (int)horizontal;
        _combatant.IsAttacking = Input.GetButtonDown("Fire1");

        if (_showDebugLog)
        {
            Debug.Log("Horizontal: " + horizontal);
            Debug.Log("Vertical: " + vertical);
            Debug.Log("Jumping: " + jump);
            Debug.Log("IsJumpCancelled: " + cancelJump);
            Debug.Log("IsDashing: " + dash);
            Debug.Log("IsAttacking: " + _combatant.IsAttacking);
        }
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public float Horizontal
    {
        get { return horizontal; }
    }

    public float Vertical
    {
        get { return vertical; }
    }
    public bool Jump
    {
        get { return jump; }
    }
    public bool CancelJump
    {
        get { return cancelJump; }
    }
    public bool Dash
    {
        get { return dash; }
    }
}
