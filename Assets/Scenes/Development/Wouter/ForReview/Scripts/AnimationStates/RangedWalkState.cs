using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class RangedWalkState : AnimationState
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
            if(_locomotion == null)
            {
                Debug.LogError("Locomotion not found on " + name);
            }
        }

        public override void OnStateEnter() { }

        public override void OnStateExecute() { }

        public override void OnStateExit() { }

        public override bool ShouldPlay()
        {
            if (Mathf.Abs(_locomotion.Velocity.x) > 0)
            {
                return true;
            }

            return false;
        }
    }
}
