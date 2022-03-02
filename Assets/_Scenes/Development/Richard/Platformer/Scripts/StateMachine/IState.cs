using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnStateEnter();
    void Update();
    void OnStateExit();
}
