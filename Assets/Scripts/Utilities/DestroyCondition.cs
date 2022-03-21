using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {
	public class DestroyCondition : MonoBehaviour {
		[SerializeField] private GameObject[] enemiesInCondition;
		[SerializeField] private GameObject[] enemiesToCheck;
		[SerializeField] private GameObject[] enemiesToCheckNext;

		private void Awake() {
			enemiesToCheck = enemiesInCondition;
			StartCoroutine(CheckEnemies());
		}


		private IEnumerator CheckEnemies() {
			while (enemiesToCheck.Length > 0) {
				enemiesToCheckNext = new GameObject[0];
				for (int i = 0; i < enemiesToCheck.Length; i++) {
					if (!enemiesToCheck[i].GetComponent<IHealth>().IsDead) {
						AddObjectToArray(ref enemiesToCheckNext, enemiesToCheck[i]);
					}
				}
				enemiesToCheck = enemiesToCheckNext;
				yield return new WaitForEndOfFrame();
			}
			DestroyThisObject();
		}

		private void DestroyThisObject() {
			this.gameObject.SetActive(false);
		}
		
		private void AddObjectToArray(ref GameObject[] refArray, GameObject newObject) {
			GameObject[] tmpArray = new GameObject[refArray.Length + 1];
			for (int i = 0; i < refArray.Length; i++) {
				tmpArray[i] = refArray[i];
			}
			tmpArray[refArray.Length] = newObject;
			refArray = tmpArray;
		}
	}
}