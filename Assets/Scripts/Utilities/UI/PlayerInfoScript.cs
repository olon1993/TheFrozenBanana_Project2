using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

public class PlayerInfoScript : MonoBehaviour
{
	// This script handles the Stats bars of the player.

	[SerializeField] private Slider hpBar, staminaBar;
	private GameObject player;
	private Health refPlayerHealth;
	private Stamina refPlayerStamina;
	private bool playerReady;

	private void Awake() {
		playerReady = false;
		StartCoroutine(LookForPlayer());
	}

	private IEnumerator LookForPlayer() {
		while (player == null) {
			GameObject p = GameObject.FindGameObjectWithTag("Player");
			if (p != null) {
				player = p;
				try {
					refPlayerHealth = player.GetComponent<Health>();
					hpBar.minValue = 0;
					hpBar.maxValue = refPlayerHealth.MaxHealth;
				} catch (Exception e) {
					Debug.Log("Player has no Health script:\n" + e);
				}
				try {
					refPlayerStamina = player.GetComponent<Stamina>();
					staminaBar.minValue = 0;
					staminaBar.maxValue = refPlayerStamina.MaxStamina;
				} catch (Exception e) {
					Debug.Log("Player has no Stamina script:\n" + e);
				}
			}
			yield return new WaitForEndOfFrame();
		}
		playerReady = true;
	}


	private void Update() {
		if (!playerReady) {
			return;
		}
		try {
			UpdateHpBar();
			UpdateStaminaBar();
		} catch (NullReferenceException nre) {
			Debug.Log("Player Object destroyed: " + nre);
		}
	}

	private void UpdateHpBar() {
		if (refPlayerHealth != null) {
			hpBar.value = refPlayerHealth.CurrentHealth;
		}
	}

	private void UpdateStaminaBar() {
		if (refPlayerStamina != null) {
			staminaBar.value = refPlayerStamina.CurrentStamina;
		}
	}
}
