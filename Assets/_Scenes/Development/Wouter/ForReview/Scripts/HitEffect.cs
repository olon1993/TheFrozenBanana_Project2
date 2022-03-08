using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class HitEffect : MonoBehaviour 
	{
        [SerializeField] private float destroyTimer;
        [SerializeField] private AudioSource heas;
        [SerializeField] private Animator heac;
        [SerializeField] private string heAnimationName;

        private void OnAwake()
        {
            if (destroyTimer == 0f)
            {
                destroyTimer = 2f;
            }
            PlayHitEffect();
        }

        private void PlayHitEffect()
        {
            if (heas != null)
            {
                heas.Play();
            }
            if (heac != null)
            {
                heac.Play(heAnimationName);
            }
            Destroy(this.gameObject, destroyTimer);
        }
    }
}
