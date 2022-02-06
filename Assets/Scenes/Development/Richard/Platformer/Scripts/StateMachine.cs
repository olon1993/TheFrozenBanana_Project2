using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
   
        IState CurrentState;

        void Update()
        {
            CurrentState.Update();
        }

        public void ChangeState(IState newState)
        {
            CurrentState.OnStateExit();
            CurrentState = newState;
            CurrentState.OnStateEnter();
        }
 }
