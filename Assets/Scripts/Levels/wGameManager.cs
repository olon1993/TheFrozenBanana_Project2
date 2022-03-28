using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class wGameManager : MonoBehaviour 
	{
		public static wGameManager gm;
		public static PlayerData pd;

		[SerializeField] private string startSceneName;
		[SerializeField] private string hubSceneName;
		[SerializeField] private string levelSceneName;
		[SerializeField] private string gameOverSceneName;
		[SerializeField] private string endSceneName;
		[SerializeField] private GameObject[] levels;
		[SerializeField] private GameObject hubLevel;
		[SerializeField] private GameObject endLevel;
		[SerializeField] private GameObject fadeScreen;

		[SerializeField] private GameObject mainCamPrefab;
		[SerializeField] private GameObject audioManagerPrefab;
		[SerializeField] private GameObject parallaxPrefab;
		[SerializeField] private GameObject uiPrefab;
		[SerializeField] private float fadeScreenTime;

		private bool initializing;
		private int levelSelected = 0;

		private GameObject camTracker;
		private GameObject audioTracker;
		private GameObject parallaxTracker;
		private GameObject levelTracker;
		private GameObject uiTracker;


		private bool faded, sceneReady;

		private void Awake() {
			if (gm == null) {
				gm = this;
			} else {
				Destroy(this.gameObject);
			}
			pd = gameObject.GetComponent<PlayerData>();
			DontDestroyOnLoad(this);
			LoadTitle();
		}

		public void LoadTitle() {
			SceneManager.LoadSceneAsync(startSceneName);
			StartCoroutine(FadeToTransparent());
		}

		public void StartupGame() {
			initializing = true;
			StartCoroutine(SwitchScene(levelSceneName));
		}

		// SCENE HOPPING AND LEVEL SELECTION

		public bool SelectLevel(int lvl) {
			levelSelected = lvl;
			bool levelExists = true;
			try {
				GameObject testLevelExists = levels[lvl];
			} catch (IndexOutOfRangeException e) {
				Debug.Log("Level does not exist: " + e);
				levelExists = false;
			}
			return levelExists;
		}

		public void ReturnToHub() {
			// This is called through the game over scene
			levelSelected = -1;
			StartCoroutine(SwitchScene(hubSceneName));
		}

		public void ReturnToHub(bool[] collected) {
			// This is called for collecting a shippart or when 
			// no shipparts are available at player's discretion
			pd.UpdateCollectibles(levelSelected, collected);
			levelSelected = -1;
			StartCoroutine(SwitchScene(hubSceneName));
		}

		public void RunLevel() {
			StartCoroutine(SwitchScene(levelSceneName));
		}

		public void RunGameOver() {
			pd.PlayerDied();
			StartCoroutine(SwitchToGameOver());
		}

		public void RunEnd() {
			levelSelected = -2;
			StartCoroutine(SwitchScene(endSceneName));
		}
		// THE SCENE SWITCHER AND FADERS

		private IEnumerator SwitchToGameOver() {
			StartCoroutine(FadeToBlack());
			while (!faded) {
				yield return new WaitForEndOfFrame();
			}
			Destroy(levelTracker);
			SceneManager.LoadSceneAsync(gameOverSceneName);
			yield return new WaitForSeconds(1f);
			UpdateFadeScreen(0);
		}

		private IEnumerator SwitchScene(string name) {
			StartCoroutine(FadeToBlack());
			while (!faded) {
				yield return new WaitForEndOfFrame();
			}
			if (initializing) {
				pd.CreateCollectibleData();
				initializing = false;
			}
			StartCoroutine(LoadSceneAndObjects(name));
			while (!sceneReady) {
				yield return new WaitForEndOfFrame();
			}
			StartCoroutine(FadeToTransparent());

		}

		IEnumerator FadeToBlack() {
			faded = false;
			sceneReady = false;
			float t = 0;
			float a = 0;
			while (t < fadeScreenTime) {
				t += Time.deltaTime;
				a = t / fadeScreenTime;
				UpdateFadeScreen(a);
				yield return new WaitForEndOfFrame();
			}
			a = 1;
			t = fadeScreenTime;
			UpdateFadeScreen(a);
			faded = true;
		}

		IEnumerator FadeToTransparent() {
			faded = true;
			float a = 1;
			float t = fadeScreenTime;
			while (t > 0) {
				t -= Time.deltaTime;
				a = t / fadeScreenTime;
				UpdateFadeScreen(a);
				yield return new WaitForEndOfFrame();
			}
			a = 0;
			UpdateFadeScreen(a);
			faded = false;
		}

		private void UpdateFadeScreen(float a) {
			SpriteRenderer spr = fadeScreen.GetComponent<SpriteRenderer>();
			Color col = new Color(0,0,0,a);
			spr.color = col;
		}

		IEnumerator LoadSceneAndObjects(string name) {
			while (!faded) {
				yield return new WaitForEndOfFrame();
			}
			SceneManager.LoadSceneAsync(name);
			yield return new WaitForSeconds(1f);
			camTracker = Instantiate(mainCamPrefab) as GameObject;
			audioTracker = Instantiate(audioManagerPrefab) as GameObject;
			if (levelSelected == -1) {
				levelTracker = Instantiate(hubLevel) as GameObject;
			} else if (levelSelected == -2) {
				levelTracker = Instantiate(endLevel) as GameObject;
			} else { 
				levelTracker = Instantiate(levels[levelSelected]) as GameObject;
				uiTracker = Instantiate(uiPrefab) as GameObject;
			}
			ILevel tmpLevel = levelTracker.GetComponent<ILevel>();
			tmpLevel.StartupLevel();
			Vector3 parallaxStart = tmpLevel.playerSpawnPoint.transform.position + Vector3.up * tmpLevel.backgroundVerticalCorrection;
			parallaxTracker = Instantiate(parallaxPrefab, parallaxStart, Quaternion.identity, null) as GameObject;
			sceneReady = true;
		}

		public GameObject[] RetrieveLevels() {
			return levels;
		}

		public int GetLevelSelected() {
			return levelSelected;
		}
	}
}

