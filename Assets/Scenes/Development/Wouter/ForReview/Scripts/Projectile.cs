using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public class Projectile : MonoBehaviour, IProjectile {

		[SerializeField] private float _velocity;
		[SerializeField] GameObject _child;
		private Damage _damage;
		private Vector3 _direction;
		private bool _active;

		private void Awake() {
			_damage = gameObject.GetComponent<Damage>();
			if (_child == null) {
				Debug.LogError(gameObject.name + " is not properly setup! Missing Child!");
			}
			if (_damage == null) {
				Debug.LogError(gameObject.name + " is not properly setup! Missing Damage!");
			}
		}

		public void TriggerCollision(Collider2D col) {
			RunCollisionCheck(col);
		}

		// This is the actual OnTriggerEnter2D, but it's called from the child
		private void RunCollisionCheck(Collider2D col) {
			if (!active) {
				return;
			}
			if (col.CompareTag("Player")) {
				return;
			}
			if (active) {
				Deactivate();
			}
			Health hpScript = col.GetComponent<Health>();
			if (hpScript != null) {
				// Do damage
				hpScript.TakeDamage(_damage);
			}
		}

		// only handles movement if active
		private void FixedUpdate() {
			if (active) {
				gameObject.transform.Translate(direction.normalized * velocity * Time.fixedDeltaTime);
			}
		}

		// Setup gives the target to go to and the rotation of the projectile renderer
		public void Setup(Vector3 start, Vector3 target, Quaternion projectileRotation) {
			direction = target - start;
			child.transform.rotation = projectileRotation;
			active = true;
		}

		// turns off the projectile. Not destroyed here, as it is destroyed in main 
		// should it miss. calling destroy here as well will make destroy be called 
		// twice and creating null reference exception. hence, deactivate.
		private void Deactivate() {
			active = false;
			gameObject.SetActive(false);
		}

		// Getters and Setters from interface
		public Damage damage {
			get { return _damage; }
			set { _damage = value; }
		}

		public GameObject child {
			get { return _child; }
			set { _child = value; }
		}

		public Vector3 direction {
			get { return _direction; }
			set { _direction = value; }
		}

		public bool active {
			get { return _active; }
			set { _active = value; }
		}

		public float velocity {
			get { return _velocity; }
			set { _velocity = value; }
		}
	}
}
