using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocomotion
{
    float HorizontalMovement { get; set; }
    float VerticalMovement { get; set; }
    Vector3 Movement { get; }
    float HorizontalLook { get; set; }
    float VerticalLook { get; set; }
    bool IsJumping { get; set; }
    bool IsJumpCancelled { get; set; }
    bool IsGrounded { get; }
    bool IsDashing { get; set; }
}
