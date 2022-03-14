using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class Collectible : MonoBehaviour, ICollectible 
	{
		[SerializeField] private ICollectible.CollectibleType _collectibleTypeDefinition;
		[SerializeField] private int _id;
		[SerializeField] private Collider2D _collectionBox;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Sprite _collectionSprite;
		[SerializeField] private float _timeAfterCollectRemove;
		private AudioSource _audioSource;
		private Vector3 _centerLocation;
		private bool _collected = false;
		private bool removeOnTouch;

		private void Awake() {
			if (_collectionBox == null) {
				Debug.LogError(gameObject.name + " does not have a collider to interact with!");
			}
			if (_spriteRenderer == null) {
				Debug.LogError(gameObject.name + "'s renderer has not been assigned!");
			}
			_audioSource = gameObject.GetComponent<AudioSource>();
			_centerLocation = gameObject.transform.position;
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.CompareTag("Player") && !_collected) {
				Collect(col.gameObject);
			}
		}

		public void Collect(GameObject player) {
			switch (_collectibleTypeDefinition) {
				case ICollectible.CollectibleType.SHIPPART:
					EventBroker.CallShipPartFound();
					break;
				case ICollectible.CollectibleType.HEALTH:
					player.GetComponent<IHealth>().AddHealth(5);
					removeOnTouch = true;
					break;
				case ICollectible.CollectibleType.STAMINA:
					player.GetComponent<Stamina>().ReplenishStamina(30);
					removeOnTouch = true;
					break;
				case ICollectible.CollectibleType.CD:
					GameObject.FindGameObjectWithTag("Level").GetComponent<ILevel>().Collect(gameObject);
					removeOnTouch = true;
					break;
				default:
					Debug.Log("Something is wrong with the collectible defitition of " + gameObject.name);
					break;
			}

			if (removeOnTouch) {
				_collected = true;
				_spriteRenderer.sprite = _collectionSprite;
				if (_audioSource != null) {
					_audioSource.Play();
				}
				StartCoroutine(RotateCollectSprite());
			}
		}

		public void CollectShipPart() {
			_collected = true;
			Destroy(this.gameObject, _timeAfterCollectRemove);
		}

		private IEnumerator RotateCollectSprite() {
			float t = 0;
			while (t < _timeAfterCollectRemove) {
				t += Time.deltaTime;
				gameObject.transform.Rotate(new Vector3(0, 0, 8));
				yield return new WaitForEndOfFrame();
			}
			gameObject.SetActive(false);
		}

	// interface getters and setters
		public ICollectible.CollectibleType collectibleTypeDefinition {
			get { return _collectibleTypeDefinition; }
		}

		public int id {
			get { return _id; }
		}

		public Collider2D collectionBox {
			get { return _collectionBox; }
		}

		public SpriteRenderer spriteRenderer {
			get { return _spriteRenderer; }
		}

		public Sprite collectionSprite {
			get { return _collectionSprite; }
		}

		public AudioSource audioSource {
			get { return _audioSource; }
		}

		public Vector3 centerLocation {
			get { return _centerLocation; }
		}

		public float timeAfterCollectRemove {
			get { return _timeAfterCollectRemove; }
		}

		public bool collected {
			get { return _collected; }
			set { _collected = value; }
		}
	}
}
