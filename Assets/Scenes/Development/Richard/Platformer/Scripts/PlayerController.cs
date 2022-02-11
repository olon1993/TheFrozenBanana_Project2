using UnityEngine;

public class PlayerController : CharacterControllerAlt
{
    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    protected override void DetermineInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        cancelJump = Input.GetButtonUp("Jump");
        dash = Mathf.Abs(horizontal) > Mathf.Epsilon && Input.GetButton("Fire3");
        attack = Input.GetButtonDown("Fire1");
    }
}
