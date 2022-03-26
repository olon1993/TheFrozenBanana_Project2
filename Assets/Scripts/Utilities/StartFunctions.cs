using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana 

{
	public class StartFunctions : MonoBehaviour {

		[SerializeField] private bool skipIntro;
		[SerializeField] private AudioSource titleAudioSource;
		[SerializeField] private AudioClip titleCrashSound;
		[SerializeField] private AudioClip titleMusic;

		[SerializeField] private GameObject titleScreen;
		[SerializeField] private GameObject titleText;
		[SerializeField] private GameObject startButton;
		[SerializeField] private GameObject creditsButton;
		[SerializeField] private GameObject creditsPage;
		[SerializeField] private Sprite startButtonDefault;
		[SerializeField] private Sprite startButtonHover;
		[SerializeField] private Sprite creditsButtonDefault;
		[SerializeField] private Sprite creditsButtonHover;

		[SerializeField] private Image bookRendererA;
		[SerializeField] private Image bookRendererB;
		[SerializeField] private float timeBetweenPages;
		[SerializeField] private float timeBetweenSprites;
		[SerializeField] private float timeTransition;
		[SerializeField] private Sprite blackSprite;
		[SerializeField] private Sprite[] comicBookSpritePageOne;
		[SerializeField] private Sprite[] comicBookSpritePageTwo;
		[SerializeField] private Sprite[] comicBookSpritePageThree;
		private Sprite[][] comicBookSprites;

		private void Awake() {
			titleText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 2500, 0);
			startButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1500, 0);
			creditsButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1500, 0);
			comicBookSprites = new Sprite[3][];
			comicBookSprites[0] = comicBookSpritePageOne;
			comicBookSprites[1] = comicBookSpritePageTwo;
			comicBookSprites[2] = comicBookSpritePageThree;
			StartCoroutine(SetupTitlePage());
		}

		private IEnumerator SetupTitlePage() {
			// Fade in
			float t = 0;
			while (t < 1) {
				t += Time.deltaTime;
				titleScreen.GetComponent<Image>().color = new Color(1,1,1,t);
				yield return new WaitForEndOfFrame();
			}
			titleScreen.GetComponent<Image>().color = new Color(1, 1, 1, 1);
			// Move title to position and play sound
			float pause = 1;
			StartCoroutine(MoveInItem(titleText, 2500, pause));
			yield return new WaitForSeconds(pause);
			StartCoroutine(TrembleItem(titleText));
			titleAudioSource.clip = titleCrashSound;
			titleAudioSource.Play();
			// Move buttons to position and play music
			pause = 0.5f;
			StartCoroutine(MoveInItem(startButton, -1000, pause));
			yield return new WaitForSeconds(pause);
			StartCoroutine(MoveInItem(creditsButton, -1000, pause));
			yield return new WaitForSeconds(pause);
			yield return new WaitForSeconds(pause);
			titleAudioSource.clip = titleMusic;
			titleAudioSource.loop = true;
			titleAudioSource.Play();
		}

		private IEnumerator MoveInItem(GameObject thing, float yValue, float time) {
			float t = 0;
			RectTransform rct = thing.GetComponent<RectTransform>();
			while (t < time) {
				t += Time.deltaTime;
				float yPos = yValue - (yValue * (t/time));
				rct.anchoredPosition = new Vector3(0, yPos, 0);
				yield return new WaitForEndOfFrame();
			}
			rct.anchoredPosition = new Vector3(0, 0, 0);
		}

		private IEnumerator TrembleItem(GameObject thing) {
			float t = 0;
			float maxTime = 2;
			RectTransform rct = thing.GetComponent<RectTransform>();
			Vector3 center = rct.anchoredPosition3D;
			Vector3 randomizer = Vector3.zero;
			float randomPosValue = 20;
			float randomRotValue = 5;
			while (t < maxTime) {
				t += Time.deltaTime;
				randomizer = new Vector3(Random.Range(-randomPosValue, randomPosValue), Random.Range(-randomPosValue, randomPosValue),0);
				rct.rotation = Quaternion.Euler(0,0,Random.Range(-randomRotValue, randomRotValue));
				rct.anchoredPosition = center + randomizer;
				yield return new WaitForEndOfFrame();
				randomPosValue = 20 - ((t/maxTime) * 20);
				randomRotValue = 5 - ((t/maxTime) * 5);
			}
			rct.rotation = Quaternion.Euler(0, 0, 0);
			rct.anchoredPosition = center;
		} 

		public void StartButton() {
			titleScreen.SetActive(false);
			startButton.SetActive(false);
			creditsButton.SetActive(false);
			titleText.SetActive(false);
			if (skipIntro) {
				wGameManager.gm.StartupGame();
				return;
			}
			StartCoroutine(PlayIntro());
		}

		public void CreditsButton(bool activate) {
			creditsPage.SetActive(activate);
		}

		public void HoverStartButton(bool mouseIn) {
			if (mouseIn) {
				startButton.GetComponent<Image>().sprite = startButtonHover;
			} else {
				startButton.GetComponent<Image>().sprite = startButtonDefault;
			}
		}

		public void HoverCreditsButton(bool mouseIn) {
			if (mouseIn) {
				creditsButton.GetComponent<Image>().sprite = creditsButtonHover;
			} else {
				creditsButton.GetComponent<Image>().sprite = creditsButtonDefault;
			}
		}

		private IEnumerator PlayIntro() {
			for (int i = 0; i < comicBookSprites.Length; i++) {
				for (int j = 0; j < comicBookSprites[i].Length; j++) {
					if (j == 0) {
						bookRendererA.sprite = blackSprite;
					} else {
						bookRendererA.sprite = comicBookSprites[i][j-1];
					}
					UpdateRenderColor(0);
					bookRendererB.sprite = comicBookSprites[i][j];
					StartCoroutine(SpriteTransition());
					yield return new WaitForSeconds(timeTransition);
					yield return new WaitForSeconds(timeBetweenSprites);
					bookRendererA.sprite = comicBookSprites[i][j];
				}
				yield return new WaitForSeconds(timeBetweenPages);
				UpdateRenderColor(0);
				bookRendererB.sprite = blackSprite;
				StartCoroutine(SpriteTransition());
				yield return new WaitForSeconds(timeTransition);
			}
			wGameManager.gm.StartupGame();
		}

		private IEnumerator SpriteTransition() {
			float t = 0;
			float a = 0;
			while (t < timeTransition) {
				t += Time.deltaTime;
				a = t / timeTransition;
				UpdateRenderColor(a);
				yield return new WaitForEndOfFrame();
			}
			UpdateRenderColor(1);
		}

		private void UpdateRenderColor(float a) {
			Color trans = new Color (1,1,1,a);
			bookRendererB.color = trans;
		}
	}
}
