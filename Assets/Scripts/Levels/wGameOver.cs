using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class wGameOver : MonoBehaviour
	{

		[SerializeField] private AudioSource gosAudio;
		[SerializeField] private AudioClip gosAudioClip;
		[SerializeField] private AudioClip gosAudioMusic;
		[SerializeField] private float buttonDelay = 3.5f;
		[SerializeField] private GameObject ButtonRestart;
		[SerializeField] private GameObject ButtonReturnToHub;

		private Animator gosAnimator;

		private void Awake()
		{
			gosAnimator = GetComponentInChildren<Animator>();
		}

        private void Start()
        {
			StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
		{
			yield return new WaitForSeconds(1f);
			gosAnimator.Play("GameOverAnimation");
			gosAudio.clip = gosAudioClip;
			gosAudio.Play();

			yield return new WaitForSeconds(buttonDelay);
			gosAudio.clip = gosAudioMusic;
			gosAudio.Play();

			int lvl = wGameManager.gm.GetLevelSelected();
			ShowButtonRestart();
			if (lvl > 0) {
				ShowButtonReturnToHub();
			}
		}

		private void ShowButtonRestart() {
			ButtonRestart.SetActive(true);
		}

		private void ShowButtonReturnToHub() {
			ButtonReturnToHub.SetActive(true);
		}
		
		public void PressRestart() {
			ButtonReturnToHub.SetActive(false);
			ButtonRestart.SetActive(false);
			wGameManager.gm.RunLevel();
		}

		public void PressReturnToHub() {
			ButtonReturnToHub.SetActive(false);
			ButtonRestart.SetActive(false);
			wGameManager.gm.ReturnToHub();
		}
	}
}
