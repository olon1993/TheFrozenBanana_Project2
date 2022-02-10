using UnityEngine;

public class PlayerInputManager : Actor
{
    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\
    
    // Movement
    public bool IsMovementEnabled = true;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

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
}
