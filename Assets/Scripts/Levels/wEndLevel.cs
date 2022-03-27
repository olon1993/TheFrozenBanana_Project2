using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class wEndLevel : wLevel {
		[SerializeField] private Canvas endCanvas;
		[SerializeField] private float delayEndCanvas;
		[SerializeField] private Image bookRendererA;
		[SerializeField] private Image bookRendererB;
		[SerializeField] private Sprite blackSprite;
		[SerializeField] private Sprite endSpriteA;
		[SerializeField] private Sprite endSpriteB;
		[SerializeField] private GameObject creditsText;
		[SerializeField] private GameObject endScoreBox;
		[SerializeField] private GameObject returnToTitleButton;

		public override void StartupLevel() {
			returnToTitleButton.SetActive(false);
			StartCoroutine(RunEndCanvas());
		}

		private IEnumerator RunEndCanvas() {
			yield return new WaitForSeconds(delayEndCanvas);
			endCanvas.gameObject.SetActive(true);
			StartCoroutine(RunTransitions());
		}

		private IEnumerator RunTransitions() {
			yield return new WaitForEndOfFrame();
			// Transition 1: Fade Screen
			bookRendererB.sprite = blackSprite;
			StartCoroutine(SpriteTransition());
			yield return new WaitForSeconds(1f);
			// Transition 2: Image 1
			bookRendererA.sprite = blackSprite;
			bookRendererB.sprite = endSpriteA;
			StartCoroutine(SpriteTransition());
			yield return new WaitForSeconds(3f);
			// Transition 2: Fade Image
			bookRendererA.sprite = endSpriteA;
			bookRendererB.sprite = blackSprite;
			StartCoroutine(SpriteTransition());
			yield return new WaitForSeconds(1f);
			// Transition 2: Fade Image
			bookRendererA.sprite = blackSprite;
			bookRendererB.sprite = endSpriteB;
			StartCoroutine(SpriteTransition());
			yield return new WaitForSeconds(2f);
			StartCoroutine((RunCredits()));

		}

		private IEnumerator SpriteTransition() {
			float t = 0;
			float a = 0;
			while (t < 1) {
				t += Time.deltaTime;
				a = t;
				UpdateRenderColor(a);
				yield return new WaitForEndOfFrame();
			}
			UpdateRenderColor(1);
		}

		private IEnumerator RunCredits() {
			yield return new WaitForEndOfFrame();
			creditsText.SetActive(true);
			yield return new WaitForSeconds(1f);
			float t = 8;
			float moveFactor = 666 * 2 / t;
			while (t > 0) {
				creditsText.transform.localPosition += Vector3.up * moveFactor * Time.deltaTime;
				yield return new WaitForEndOfFrame();
				t -= Time.deltaTime;
			}
			ShowEndScore();
			EnableReturnToTitle();
		}

		private void ShowEndScore() {
			endScoreBox.SetActive(true);
			Text[] scoreTexts = endScoreBox.GetComponentsInChildren<Text>();
			string scoreString = CountScore() + "% completed";
			foreach (Text txt in scoreTexts) {
				txt.text = scoreString;
			}
		}

		private string CountScore() {
			float score = 0;
			int countTotal = 0;
			int countCollected = 0;
			bool[][] counter = wGameManager.pd.RetrieveAllStatus();
			for (int i = 0; i < counter.Length; i++) {
				for (int j = 0; j < counter[i].Length; j++) {
					countTotal++;
					if (counter[i][j]) {
						countCollected++;
					}
				}
			}
			score = Mathf.Round(((float)countCollected / (float)countTotal) * 100);
			return score.ToString();
		}

		private void UpdateRenderColor(float a) {
			Color trans = new Color(1, 1, 1, a);
			bookRendererB.color = trans;
		}

		private void EnableReturnToTitle() {
			returnToTitleButton.SetActive(true);
		}

		public void ReturnToTitleScreen() {
			wGameManager.gm.LoadTitle();
		}
	}
}
