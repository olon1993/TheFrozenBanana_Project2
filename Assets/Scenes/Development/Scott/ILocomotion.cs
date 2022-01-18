using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocomotion
{
    float HorizontalMovement { get; set; }
    float VerticalMovement { get; set; }
    bool IsJumping { get; set; }
    bool IsJumpCancelled { get; set; }
    bool IsDashing { get; set; }
}
