using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{ 
    public class DashState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ILocomotion _locomotion;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Awake()
        {
            _locomotion = GetComponentInParent<ILocomotion>();
            if (_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override void OnStateEnter() { }

        public override void OnStateExecute() { }

        public override void OnStateExit() { }

        public override bool ShouldPlay()
        {
            if (_locomotion.IsDashing && Mathf.Abs(_locomotion.Velocity.x) > Mathf.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
