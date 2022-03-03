using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana 
{
	public class Collectible : MonoBehaviour, ICollectible 
	{
		[SerializeField] private ICollectible.CollectibleType _collectibleTypeDefinition;
		[SerializeField] private Collider2D _collectionBox;
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Sprite _collectionSprite;
		[SerializeField] private float _moveHorizontal;
		[SerializeField] private float _moveVertical;
		[SerializeField] private float _moveTime;
		[SerializeField] private float _timeAfterCollectRemove;
		private AudioSource _audioSource;
		private Vector3 _centerLocation;
		private bool _collected = false;

		private void Awake() {
			if (_collectionBox == null) {
				Debug.LogError(gameObject.name + " does not have a collider to interact with!");
			}
			if (_spriteRenderer == null) {
				Debug.LogError(gameObject.name + "'s renderer has not been assigned!");
			}
			_audioSource = gameObject.GetComponent<AudioSource>();
			_centerLocation = gameObject.transform.position;
			StartCoroutine(FloatMovement());
		}

		private void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.CompareTag("Player") && !_collected) {
				_collected = true;
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
					break;
				case ICollectible.CollectibleType.STAMINA:
					player.GetComponent<Stamina>().ReplenishStamina(30);
					break;
				case ICollectible.CollectibleType.CD:
					Debug.Log("CD collected. Now we just have to do something here");
					break;
				default:
					Debug.Log("Something is wrong with the collectible defitition of " + gameObject.name);
					break;
			}
			_spriteRenderer.sprite = _collectionSprite;
			if (_audioSource != null) {
				_audioSource.Play();
			}
			Destroy(this.gameObject, _timeAfterCollectRemove);
		}

		public IEnumerator FloatMovement() {
			float x = 0;
			float y = 0;
			float t = 0;
			float sign = 1;
			while (true) {
				t += Mathf.Sign(sign) * (Time.deltaTime * _moveTime) / _moveTime;
				if (t > 1) {
					t = 1;
					sign = -1;
				} else if (t < 0) {
					t = 0;
					sign = 1;
				}
				x = Mathf.Lerp(-_moveHorizontal, _moveHorizontal, t);
				y = Mathf.Lerp(-_moveVertical, _moveVertical, t);
				gameObject.transform.position = new Vector3(_centerLocation.x + x, _centerLocation.y + y, _centerLocation.z);
				if (_collected) {
					gameObject.transform.Rotate(new Vector3(0,0,8));
				}
				yield return new WaitForEndOfFrame();
			}
		} 

		// interface getters and setters
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

		public float moveHorizontal {
			get { return _moveHorizontal; }
		}

		public float moveVertical {
			get { return _moveVertical; }
		}

		public float moveTime {
			get { return _moveTime; }
		}

		public float timeAfterCollectRemove {
			get { return _timeAfterCollectRemove; }
		}

		public bool collected {
			get { return _collected; }
		}
	}
}
