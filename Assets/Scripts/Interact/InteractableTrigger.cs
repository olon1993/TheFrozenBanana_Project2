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
		[SerializeField] bool _pauseUntilDismissed;
		[SerializeField] string _buttonToDismissDialogue;
		private int timesTriggered;

		//**************************************************\\
		//******************** Methods *********************\\
		//**************************************************\\

		protected override void Update()
		{
			if (Input.GetButtonDown("Interact") && interactTextIsActive)
			{
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
				if (_pauseUntilDismissed)
                {
					StartCoroutine(PauseGameUntilKeyPressed());
                }
				Interact();
			}
		}

		private IEnumerator PauseGameUntilKeyPressed()
        {
			EventBroker.CallPauseGame();
			bool keyPressed = false;
			while (!keyPressed)
            {
				yield return new WaitForEndOfFrame();
				if (Input.GetButton(_buttonToDismissDialogue))
				{
					EventBroker.CallUnpauseGame();
					keyPressed = true;
				}
			}
			interactTextBox.SetActive(false);
			interactTextIsActive = false;
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