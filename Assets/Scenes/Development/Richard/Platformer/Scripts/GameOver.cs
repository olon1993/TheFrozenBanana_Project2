using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class GameOver : MonoBehaviour
	{

		[SerializeField] private AudioSource gosAudio;
		[SerializeField] private AudioClip gosAudioClip;
		[SerializeField] private GameObject _anyKeyText;

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
			yield return new WaitForSeconds(1);
			gosAnimator.Play("GameOverAnimation");
			gosAudio.clip = gosAudioClip;
			gosAudio.Play();

			yield return new WaitForSeconds(5);

			if (_anyKeyText != null) _anyKeyText.SetActive(true);

			bool keyPressed = false;
			while (!keyPressed)
			{
				if (Input.anyKey)
				{
					keyPressed = true;
					GameManager.Instance.RestartCurrentLevel();
				}
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
