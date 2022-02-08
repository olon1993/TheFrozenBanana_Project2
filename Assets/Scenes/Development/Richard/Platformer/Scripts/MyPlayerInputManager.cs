using UnityEngine;

public class MyPlayerInputManager : MonoBehaviour, IInputManager
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Inputs
    float horizontal;
    float vertical;
    bool jump, cancelJump;
    bool dash;
    bool attack;

    // Movement
    public bool IsMovementEnabled = true;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        cancelJump = Input.GetButtonUp("Jump");
        dash = Mathf.Abs(horizontal) > Mathf.Epsilon && Input.GetButton("Fire3");
        attack = Input.GetButtonDown("Fire1");

        if (_showDebugLog)
        {
            Debug.Log("Horizontal: " + horizontal);
            Debug.Log("Vertical: " + vertical);
            Debug.Log("Jumping: " + jump);
            Debug.Log("IsJumpCancelled: " + cancelJump);
            Debug.Log("IsDashing: " + dash);
            Debug.Log("IsAttacking: " + attack);
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
    public bool Attack
    {
        get { return attack; }
    }
}
