using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana 
{ 
	public class ProgressOverview : MonoBehaviour {

		[SerializeField] private float transformTime;
		[SerializeField] private Animator screenAC;
		[SerializeField] private Canvas textCanvas;
		[SerializeField()]
		private Text deathText, cdText, shippartText;

		private Transform trans;
		private Transform focalPoint;

		private int deathCount, cdCount, totalCds, shippartCount, totalShipparts;

		private void Awake() {
			trans = this.gameObject.transform;
			textCanvas.enabled = false;
		}

		void OnEnable() {
			RetrieveData();
			focalPoint = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
			trans.position = focalPoint.position + 4*Vector3.right + 2*Vector3.up;
			trans.localScale = new Vector3(0.1f, 0.1f, 1f);
			StartCoroutine(StartupScreen());
		}


		void OnDisable() {
			trans.localScale = new Vector3(0, 0, 0);
			textCanvas.enabled = false;
		}

		private void RetrieveData() {
			cdCount = 0;
			totalCds = 0;
			shippartCount = 0;
			totalShipparts = 0;
			deathCount = wGameManager.pd.RetrieveDeaths();
			bool[][] tmpStatus = wGameManager.pd.RetrieveAllStatus();
			ICollectible.CollectibleType[][] tmpTypes = wGameManager.pd.RetrieveAllTypes();
			for (int i = 0; i < tmpStatus.Length; i++) {
				for (int j = 0; j < tmpStatus[i].Length; j++) {
					if (tmpTypes[i][j] == ICollectible.CollectibleType.CD) {
						totalCds++;
						if (tmpStatus[i][j] == true) {
							cdCount++;
						}
					}
					if (tmpTypes[i][j] == ICollectible.CollectibleType.SHIPPART) {
						totalShipparts++;
						if (tmpStatus[i][j] == true) {
							shippartCount++;
						}
					}
				}
			}
			deathText.text = deathCount.ToString();
			cdText.text = cdCount.ToString() + " / " + totalCds.ToString();
			shippartText.text = shippartCount.ToString() + " / " + totalShipparts.ToString();
		}

		private IEnumerator StartupScreen() {
			float t = 0f;
			float x = 0.1f;
			float y = 0.1f;
			float z = 1f;
			yield return new WaitForEndOfFrame();
			// Widen
			while (t < transformTime) {
				t += Time.deltaTime;
				x = t / transformTime;
				trans.localScale = new Vector3(x, y, z);
				yield return new WaitForEndOfFrame();
			}
			x = 1f;
			t = 0f;
			// Heighten
			while (t < transformTime) {
				t += Time.deltaTime;
				y = t / transformTime;
				trans.localScale = new Vector3(x, y, z);
				yield return new WaitForEndOfFrame();
			}
			y = 1f;
			trans.localScale = new Vector3(x, y, z);
			yield return new WaitForSeconds(0.2f);
			textCanvas.enabled = true;
		}
	}


}