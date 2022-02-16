using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{ 
    public class AttackState : AnimationState
    {

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        private ICombatant _combatant;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Awake()
        {
            _combatant = GetComponentInParent<ICombatant>();
            if (_combatant == null)
            {
                Debug.LogError("Combatant not found on " + name);
            }
        }

        public override void OnStateEnter() { }

        public override void OnStateExecute() { }

        public override void OnStateExit() { }

        public override bool ShouldPlay()
        {
            if (_combatant.IsAttacking)
            {
                return true;
            }

            return false;
        }

        public override bool CanInterrupt => false;
    }
}
