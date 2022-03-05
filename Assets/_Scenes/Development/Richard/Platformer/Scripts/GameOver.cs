using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;
using TMPro;

namespace TheFrozenBanana
{
	public class GameOver : MonoBehaviour
	{

		[SerializeField] private AudioSource gosAudio;
		[SerializeField] private AudioClip gosAudioClip;
		[SerializeField] private GameObject _continueTextGO;
		[SerializeField] private TextMeshProUGUI _countdownText;
		[SerializeField] private int _countdownTime = 10;

		private Animator gosAnimator;

		private void Awake()
		{
			gosAnimator = GetComponentInChildren<Animator>();
			_continueTextGO.SetActive(false);
		}

        private void Start()
        {
			StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
		{
			yield return new WaitForSeconds(1);
			gosAnimator.Play("GameOverAnimation");
			gosAudio.clip = gosAudioClip;
			gosAudio.Play();

			yield return new WaitForSeconds(5);

			StartCoroutine(ContinueCountdown());

			bool keyPressed = false;
			while (!keyPressed)
			{
				if (Input.anyKey)
				{
					keyPressed = true;
					GameManager.Instance.LoadHubScene();
				}
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator ContinueCountdown()
        {
			if (_continueTextGO == null) yield break;

			_continueTextGO.SetActive(true);

			var timer = _countdownTime;
			_countdownText.text = timer.ToString();
			while (timer > 0)
            {
				yield return new WaitForSeconds(1);
				timer -= 1;
				_countdownText.text = timer.ToString();
			}

			GameManager.Instance.ReloadGame();
		}
	}
}
