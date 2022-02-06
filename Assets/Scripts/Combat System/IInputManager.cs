using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager
{
    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    float Horizontal { get; }
    float Vertical { get; }
    bool Jump { get; }

    bool CancelJump { get; }
    bool Dash { get; }
}
