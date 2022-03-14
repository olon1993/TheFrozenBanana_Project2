using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class wLevel : MonoBehaviour, ILevel {
		[SerializeField] private GameObject _playerPrefab;
		[SerializeField] private GameObject _playerSpawnPoint;
		[SerializeField] private GameObject _teleportFieldLevelComplete;
		[SerializeField] private GameObject _levelCompleteTextBox;
		[SerializeField] private int _levelNumber;
		[SerializeField] private GameObject[] _collectibles;
		[SerializeField] private GameObject teleportationEffect;
		private GameObject player;

		private bool[] tmpStatusCollected;
		private bool _levelComplete;
		private bool _levelAccessible;
		private GameObject _teleportField;

		public void StartupLevel() {
			player = Instantiate(_playerPrefab, _playerSpawnPoint.transform.position, Quaternion.identity, null) as GameObject;
			if (_levelNumber > -1) {
				CollectibleStatusCheck();
			}
		}

		private void CollectibleStatusCheck() { 
			int shipPartCount = 0;
			int cdCount = 0;
			tmpStatusCollected = wGameManager.pd.RetrieveLevelObjectStatus(_levelNumber);
			for (int i = 0; i < _collectibles.Length; i++) {
				_collectibles[i].GetComponent<ICollectible>().collected = tmpStatusCollected[i];
				if (_collectibles[i].GetComponent<ICollectible>().collected) {
					_collectibles[i].SetActive(false);
				} else {
					if (_collectibles[i].GetComponent<ICollectible>().collectibleTypeDefinition == ICollectible.CollectibleType.SHIPPART) {
						shipPartCount++;
					} else if (_collectibles[i].GetComponent<ICollectible>().collectibleTypeDefinition == ICollectible.CollectibleType.CD) {
						cdCount++;
					}
				}
			}
			if (shipPartCount == 0) {
				levelComplete = true;
				_teleportField = Instantiate(_teleportFieldLevelComplete, player.transform.position, Quaternion.identity, player.transform) as GameObject;
			}
		}

		public void Collect(GameObject collectible) {
			for (int i = 0; i < _collectibles.Length; i++) {
				if (_collectibles[i].GetComponent<ICollectible>().id == collectible.GetComponent<ICollectible>().id) {
					tmpStatusCollected[i] = true;
					if (collectible.GetComponent<ICollectible>().collectibleTypeDefinition == ICollectible.CollectibleType.SHIPPART) {
						collectible.GetComponent<ICollectible>().CollectShipPart();
						StartCoroutine(CloseLevel());
					}
				}
			}
		}

		public void ExitLevel() {
			StartCoroutine(CloseLevel());
		}

		public void CreateTeleportEffect() {
			StartCoroutine(RunTeleportEffect());
		}

		private IEnumerator RunTeleportEffect() {
			GameObject tmpEffect = Instantiate(teleportationEffect, player.transform.position, Quaternion.identity, player.transform) as GameObject;
			yield return new WaitForSeconds(0.5f);
			tmpEffect.transform.SetParent(null);
			player.SetActive(false);

		}

		private IEnumerator CloseLevel() {
			CreateTeleportEffect();
			yield return new WaitForSeconds(1f);
			wGameManager.gm.ReturnToHub(tmpStatusCollected);
		}

		// Interface getters and setters
		public GameObject playerPrefab {
			get { return _playerPrefab; }
		}

		public GameObject playerSpawnPoint {
			get { return _playerSpawnPoint; }
		}

		public GameObject[] collectibles {
			get { return _collectibles; }
		}

		public GameObject levelCompleteTextBox {
			get { return _levelCompleteTextBox; }
		}

		public bool levelComplete {
			get { return _levelComplete; }
			set { _levelComplete = true; }
		}

		public bool levelAccessible {
			get { return _levelAccessible; }
			set { _levelAccessible = value; }
		}
	}
}
