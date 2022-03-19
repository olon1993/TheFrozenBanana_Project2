using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class LevelSelectionScript : MonoBehaviour {

		[SerializeField] private GameObject confirmWindow;
		[SerializeField] private Text confirmTextBox;
		[SerializeField] private Text collectibleTextBox;
		[SerializeField] private Slider levelDifficulty;
		private string confirmationText;
		private string standardText;
		
		private void Awake() {
			standardText = "Are you sure you want to go to level ";
		}

		private void OnEnable() {
			confirmWindow.SetActive(false);
		}

		private void OnDisable() {
			if (confirmWindow != null) {
				confirmWindow.SetActive(false);
			}
		}

		void Update() {
			if (Input.GetMouseButtonDown(0)) {
				SelectionRay();
			}
		}


		void SelectionRay() {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// requires 3D collider to hit unfortunately...
			if (Physics.Raycast(ray, out hit)) {
				LevelInformation li = hit.transform.GetComponent<LevelInformation>();
				if (wGameManager.gm.SelectLevel(li.GetLevelId())) {
					ActivateConfirmWindow(li.GetLevelId(), li.GetLevelDifficulty());
				}
			}
		}

		private void ActivateConfirmWindow(int lvlId, int difficulty) {
			confirmationText = standardText + lvlId.ToString() + "?";
			confirmTextBox.text = confirmationText;
			collectibleTextBox.text = CheckCollectibles(lvlId);
			levelDifficulty.value = difficulty;
			confirmWindow.SetActive(true);
		}

		public void ConfirmChoice() {
			GameObject.FindGameObjectWithTag("Level").GetComponent<ILevel>().CreateTeleportEffect();
			wGameManager.gm.RunLevel();
			this.gameObject.SetActive(false);
		}

		public void CloseConfirmWindow() {
			wGameManager.gm.SelectLevel(-1);
			confirmWindow.SetActive(false);
		}


		private string CheckCollectibles(int lvlId) {
			int shipPartCount = 0;
			int cdCount = 0;
			bool[] tmpStatusCollected = wGameManager.pd.RetrieveLevelObjectStatus(lvlId);
			ICollectible.CollectibleType[] tmpTypeCollected = wGameManager.pd.RetrieveLevelObjectTypes(lvlId);
			for (int i = 0; i < tmpStatusCollected.Length; i++) {
				if (tmpStatusCollected[i]) {
					continue;
				}
				if (tmpTypeCollected[i] == ICollectible.CollectibleType.SHIPPART) {
					shipPartCount++;
				} else if (tmpTypeCollected[i] == ICollectible.CollectibleType.CD) {
					cdCount++;
				}
			}
			string collectibleString = "";
			if (cdCount == 0 && shipPartCount == 0) {
				collectibleString = "Nothing left to collect here.";
				return collectibleString;
			} else {
				if (shipPartCount > 0) {
					collectibleString = "Ship parts left: " + shipPartCount.ToString() + "\n";
				}
				if (cdCount > 0) {
					collectibleString += "CD's left: " + cdCount.ToString();
				}
			}
			return collectibleString;
		}
	}
}