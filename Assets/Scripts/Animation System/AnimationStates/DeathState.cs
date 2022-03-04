using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana {
	public class DeathState : AnimationState {

		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		// Dependencies
		private IHealth _health;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		private void Awake() {
			_health = GetComponentInParent<IHealth>();
			if (_health == null) {
				Debug.LogError("Health not found on " + name);
			}
		}

		public override bool ShouldPlay() {
			if (_health.CurrentHealth <= 0f) {
				return true;
			}

			return false;
		}
	}
}
