using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana
{
	public class EndMovement : MonoBehaviour {
		[SerializeField] private float pauseMovement;
		[SerializeField] private float movementTime;
		[SerializeField] private float timeBeforeRemoval;
		[SerializeField] private Vector3 startLocation;
		[SerializeField] private Vector3 endLocation;
		[SerializeField] private Animator ac;
		[SerializeField] private string[] endAnimation;

		private Transform trans;

		private void Awake() {
			trans = gameObject.transform;
			trans.position = startLocation;
			StartCoroutine(RunMovement());
		}

		private IEnumerator RunMovement() {
			yield return new WaitForSeconds(pauseMovement);
			PlayAnimation(0);
			float t = 0;
			while (t < movementTime) {
				float mR = t / movementTime;
				trans.position = Vector3.Lerp(startLocation,endLocation, mR);
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			trans.position = endLocation;
			PlayAnimation(1);
			yield return new WaitForSeconds(timeBeforeRemoval);
			Destroy(this.gameObject);
		}
		
		private void PlayAnimation(int id) {
			if (ac == null) {
				return;
			}
			if (id < endAnimation.Length) {
				ac.Play(endAnimation[id]);
			}

		}
	}
}
