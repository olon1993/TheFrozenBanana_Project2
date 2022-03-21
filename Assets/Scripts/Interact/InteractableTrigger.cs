using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	public class InteractableTrigger : Interactable {
		//**************************************************\\
		//********************* Fields *********************\\
		//**************************************************\\
		[SerializeField] private int triggerAmount;
		private int timesTriggered;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected override void Update() {
			if (Input.GetButtonDown("Interact") && interactTextIsActive) {
				interactTextBox.SetActive(false);
				interactTextIsActive = false;
			}

		}

		protected override void OnTriggerEnter2D(Collider2D col) {
			if (timesTriggered >= triggerAmount) {
				return;
			}
			if (col.gameObject.CompareTag("Player") && !interactTextIsActive) {
				timesTriggered++;
				interactTextBox.SetActive(true);
				interactTextIsActive = true;
				Interact();
			}
		}

		protected override void OnTriggerExit2D(Collider2D col) {
			if (col.gameObject.CompareTag("Player")) {
				interactTextBox.SetActive(false);
				interactTextIsActive = false;
				interactibleActive = false;
			}
		}
	}
}