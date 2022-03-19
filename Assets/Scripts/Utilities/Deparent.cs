using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {
	public class Deparent : MonoBehaviour {
		private void Awake() {
		//	StartCoroutine(WaitToOrphan());
		}

		IEnumerator WaitToOrphan() {
			// Delay is separated so not all deparents are carried out simultaneously
			float s = Random.Range(1f,5f);
			yield return new WaitForSeconds(s);
			gameObject.transform.SetParent(null);
		}
	}
}