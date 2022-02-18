using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameOverScript : MonoBehaviour
{
	[SerializeField] private Image blackScreen;
	[SerializeField] private Text gameOverTextBox;
	[SerializeField] private AudioSource gosAudio;
	[SerializeField] private AudioClip gosAudioClip;
	[SerializeField] private float screenFadeTime, textDeltaTime;
	private string gameOverDisplayText;
	private string gameOverText = "GAME OVER";

	public void RunGameOver() {
		StartCoroutine(GameOver());
	}

	private IEnumerator GameOver() {
		// Fade-in black screen
		float t = 0f;
		Color fadeColor;
		while (t < screenFadeTime) {
			t += Time.deltaTime;
			float a = t / screenFadeTime;
			fadeColor = new Color(0,0,0, a);
			blackScreen.color = fadeColor;
			yield return new WaitForEndOfFrame();
		}
		fadeColor = new Color(0,0,0,1);
		blackScreen.color = fadeColor;

		// Text writing
		int i = 1;
		gosAudio.clip = gosAudioClip;
		while (i < gameOverText.Length + 1) {
			UpdateGameOverText(i);
			i++;
			yield return new WaitForSeconds(textDeltaTime);
		}
	}

	private void UpdateGameOverText(int length) {
		gosAudio.Play();
		gameOverDisplayText = gameOverText.Substring(0,length);
		gameOverTextBox.text = gameOverDisplayText;
	}
}
