using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana {
	public class InteractableTeleportField : Interactable {

		[SerializeField] private Collider2D triggerCollider;
		private int count;

		protected void Awake() {
			base.interactTextBox = GameObject.FindGameObjectWithTag("Level").GetComponent<ILevel>().levelCompleteTextBox;
			triggerCollider.enabled = true;
		}

		protected override void Interact() {
			count++;
			if (count > 1) {
				GameObject.FindGameObjectWithTag("Level").GetComponent<ILevel>().ExitLevel();
			}
		}
	}
}