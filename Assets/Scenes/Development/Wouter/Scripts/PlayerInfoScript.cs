using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScript : MonoBehaviour
{
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
					Debug.Log("Player has no Health script");
				}
				try {
					refPlayerStamina = player.GetComponent<Stamina>();
					staminaBar.minValue = 0;
					staminaBar.maxValue = refPlayerStamina.MaxStamina;
				} catch (Exception e) {
					Debug.Log("Player has no Stamina script");
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
		UpdateHpBar();
		UpdateStaminaBar();
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
