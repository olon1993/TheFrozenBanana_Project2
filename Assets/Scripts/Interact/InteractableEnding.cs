using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	public class InteractableEnding : Interactable {
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\


		protected override void Interact() {
			interactibleActive = !interactibleActive;
			if (interactibleObject != null) {
				interactibleObject.SetActive(interactibleActive);
			}
			wGameManager.gm.RunEnd();
		}
	}
}