using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTexts : MonoBehaviour
{
	private string[][] tooltipTexts;

	void Awake() {
		SetupTooltipTexts();
	}

	public string RetrieveTooltipText(int id, int index) {
		string txt = tooltipTexts[id][index];
		return txt;
	}

	private void SetupTooltipTexts() {
		tooltipTexts = new string[8][];
		tooltipTexts[0] = new string[4];
		tooltipTexts[1] = new string[4];
		tooltipTexts[2] = new string[4];
		tooltipTexts[3] = new string[4];
		tooltipTexts[4] = new string[2];
		tooltipTexts[5] = new string[2];
		tooltipTexts[6] = new string[2];
		tooltipTexts[7] = new string[2];

		// Guns
		tooltipTexts[0][0] = "The firing rate of the guns.";
		tooltipTexts[0][1] = "The velocity of each bullet fired.";
		tooltipTexts[0][2] = "The impact damage each bullet does.";
		tooltipTexts[0][3] = "The amount of guns on the ship.";

		// Cannon
		tooltipTexts[1][0] = "The firing rate of the cannon.";
		tooltipTexts[1][1] = "The velocity of each shot fired.";
		tooltipTexts[1][2] = "The damage each cannon shot does.";
		tooltipTexts[1][3] = "The explosive radius of the cannon.";

		// Laser
		tooltipTexts[2][0] = "The firing rate of the laser.";
		tooltipTexts[2][1] = "The velocity of the laser fired.";
		tooltipTexts[2][2] = "The damage of the laser shots.";
		tooltipTexts[2][3] = "The prism splits the laser into multiple beams.";

		// Rockets
		tooltipTexts[3][0] = "The firing rate of rockets.";
		tooltipTexts[3][1] = "The velocity of each rocket.";
		tooltipTexts[3][2] = "The damage of the rockets.";
		tooltipTexts[3][3] = "The amount of rocket pods on the ship.";

		// Hull
		tooltipTexts[4][0] = "The ship's armor. Lowers incoming damage.";
		tooltipTexts[4][1] = "The ship's hull. Increases HP.";

		// Shields
		tooltipTexts[5][0] = "Increases the maximum shield.";
		tooltipTexts[5][1] = "Increases the shield recovery rate.";

		// Engines
		tooltipTexts[6][0] = "Increases engine capactiy, allowing for longer missions.";
		tooltipTexts[6][1] = "Increases the thrust of the engines, making the ship faster.";

		// Coolers
		tooltipTexts[7][0] = "Increases the maximum heat the ship can take.";
		tooltipTexts[7][1] = "Increases the rate at which heat is cooled.";
	}
}
