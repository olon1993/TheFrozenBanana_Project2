using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUIScript : MonoBehaviour {

	/*******************************************************************
	 *  PLAY UI
	 *	This is the UI manager within the levels and play.
	 *	Here the information of the ship is shown
	 *	Other information necessary to show during game is here too
	 *	Also the graphics for the end of the level are done here (NOT YET THOUGH)
	 *
	 *******************************************************************
	 *	VERSIONS
	 *	v		By	Desc
	 *	0.1     WH  Added basic functionality
	 *******************************************************************/

	// editor setup variables
	public GameObject victoryBox, defeatBox;
	public Slider hpBar, shieldBar, heatBar, timeBar;
	public Text cashText;
	

	public void SetupInitialValues(float hp, float shield, float heat, float time) {
		hpBar.minValue = 0;
		hpBar.maxValue = hp;
		hpBar.value = hp;
		shieldBar.minValue = 0;
		shieldBar.maxValue = shield;
		if (shield < 0.1f) {
			shieldBar.value = 0;
		} else {
			shieldBar.value = shield;
		}
		heatBar.minValue = 0;
		heatBar.maxValue = heat;
		heatBar.value = 0;

		timeBar.minValue = 0;
		timeBar.maxValue = time;
		timeBar.value = time;
	}

	public void UpdateHpValue(float hp) {
		hpBar.value = hp;
	}

	public void UpdateShieldValue(float shield) {
		shieldBar.value = shield;
	}

	public void UpdateHeatValue(float heat) {
		heatBar.value = heat;
	}

	public void UpdateTimeValue(float time) {
		timeBar.value = time;
	}

	public void UpdateCashText(int cash) {
		cashText.text = cash.ToString();
	}
	
	public void ShowEndBox(bool victory) {
		if (victory) {
			victoryBox.SetActive(true);
		} else {
			defeatBox.SetActive(true);
		}
	}
}
