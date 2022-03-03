using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class WalkState : AnimationState
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
