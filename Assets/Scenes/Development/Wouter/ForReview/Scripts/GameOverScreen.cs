using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class GameOverScreen : MonoBehaviour 
		// Transform variant
		{
		[SerializeField] private SpriteRenderer blackScreen;
		[SerializeField] private float screenFadeTime;
		[SerializeField] private AudioSource gosAudio;
		[SerializeField] private AudioClip gosAudioClip;
		private Transform playerTrans;
		private Vector3 gameOverLocation;

		private Animator gosAC;

		private void Awake() {
			gosAC = GetComponentInChildren<Animator>();
			StartCoroutine(FindPlayer());
		}

		public void RunGameOver() {
			if (playerTrans != null) {
				gameOverLocation = playerTrans.position;
			}
			gameObject.transform.position = gameOverLocation;
			StartCoroutine(GameOver());
		}

		private IEnumerator FindPlayer() {
			while (playerTrans == null) {
				yield return new WaitForSeconds(1);
				playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
			}
		}

		private IEnumerator GameOver() {
			// Fade-in black screen
			float t = 0f;
			Color fadeColor;
			while (t < screenFadeTime) {
				t += Time.deltaTime;
				float a = t / screenFadeTime;
				fadeColor = new Color(0, 0, 0, a);
				blackScreen.color = fadeColor;
				yield return new WaitForEndOfFrame();
			}
			fadeColor = new Color(0, 0, 0, 1);
			blackScreen.color = fadeColor;
			gosAC.Play("GameOverAnimation");
		}
	}
}