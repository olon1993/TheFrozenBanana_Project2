using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    private ILocomotion _locomotion;
    
    // Movement
    public bool IsMovementEnabled = true;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    // Start is called before the first frame update
    void Awake()
    {
        _locomotion = transform.GetComponent<ILocomotion>();
        //_animationStateManager = transform.GetComponent<IAnimationManager>();

        if (_locomotion == null)
        {
            Debug.LogError("ILocomotion not found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _locomotion.HorizontalMovement = Input.GetAxisRaw("Horizontal");
        _locomotion.VerticalMovement = Input.GetAxisRaw("Vertical");
        _locomotion.IsJumping = Input.GetButtonDown("Jump");
        _locomotion.IsJumpCancelled = Input.GetButtonUp("Jump");
        _locomotion.IsDashing = Input.GetButtonDown("Fire3");
    }
}
