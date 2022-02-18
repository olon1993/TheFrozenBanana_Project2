using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public class wRangedWeapon : MonoBehaviour, IRangedWeapon {

		// From IRRangedWeapon
		[SerializeField] private GameObject _projectile;
		[SerializeField] private Transform _rotationalCenter;
		[SerializeField] private Transform _target;
		[SerializeField] private Renderer _weaponObject;
		[SerializeField] private Vector3 _localScale;
		[SerializeField] private bool _canAim;
		[SerializeField] private float _projectileKillTime;

		// From IWeapon
		[SerializeField] private bool _isLimitedAmmo;
		[SerializeField] private int _maxAmmo;
		[SerializeField] private int _currentAmmo;
		[SerializeField] private IWeapon.AmmoType _ammoTypeDefinition;
		[SerializeField] private Transform _pointOfOrigin;
		[SerializeField] private float _attackActionTime;


		private ILocomotion _locomotion;

		private void Awake() {
			_locomotion = gameObject.GetComponent<ILocomotion>();
			if (_locomotion == null) {
				Debug.LogError(gameObject.name + " has a ranged weapon that cannot find the owner's locomotion!");
			}
			if (_canAim && _target == null) {
				Debug.LogError(gameObject.name + "has a weapon that can aim but no target finder!");
			}
			if (_localScale == Vector3.zero) {
				_localScale = new Vector3(1,1,1);
			}
			if (projectileKillTime == 0) {
				projectileKillTime = 3f;
			}
		}

		public void Attack() {

		}

		private void UpdateRotation() {
			// need 2 vectors: the base vector (which is the look vector of the player)
			// and the vector to the target position
			Vector2 vBase = new Vector2(_locomotion.HorizontalLook, 0);
			float angle = 0;
			if (canAim) {
				Vector2 vDir = target.position - gameObject.transform.position;
				angle = Vector2.Angle(vBase, vDir) * _locomotion.HorizontalLook;
				if (target.position.y < gameObject.transform.position.y) {
					angle = -angle;
				}
			}
			weaponObject.transform.localScale = new Vector3(_locomotion.HorizontalLook * localScale.x, localScale.y, localScale.z);
			weaponObject.transform.rotation = Quaternion.Euler(0, 0, angle);
		}

		private void SpawnProjectile() {
			Quaternion rot = weaponObject.transform.rotation;
			if (_locomotion.HorizontalLook < 0) {
				rot *= Quaternion.Euler(0, 0, 180);
			}
			GameObject proj = Instantiate(projectile, PointOfOrigin.position, Quaternion.identity, null) as GameObject;
			Vector3 fireTowards = new Vector3(_locomotion.HorizontalLook, 0,0);
			if (canAim) {
				fireTowards = target.position;
			}
			proj.GetComponent<IProjectile>().Setup(gameObject.transform.position, fireTowards, rot);
			Destroy(proj, 3f);
		}


		// Getters and Setters from Interfaces
		// IRangedWeapon
		public GameObject projectile {
			get { return _projectile; }
		}

		public Transform rotationalCenter {
			get { return _rotationalCenter; }
		}

		public Transform target {
			get { return _target; }
			set { _target = value; }
		}

		public Renderer weaponObject {
			get { return _weaponObject; }
		}

		public Vector3 localScale {
			get { return _localScale; }
		}

		public bool canAim {
			get { return _canAim; }
			set { _canAim = value; }
		}

		public float projectileKillTime {
			get { return _projectileKillTime; }
			set { _projectileKillTime = value; }
		}

		// IWeapon
		public Transform PointOfOrigin {
			get { return _pointOfOrigin; }
			set { _pointOfOrigin = value; }
		}

		public bool IsLimitedAmmo {
			get { return _isLimitedAmmo; }
			set { _isLimitedAmmo = value; }
		}

		public int MaxAmmo {
			get { return _maxAmmo; }
			set { _maxAmmo = value; }
		}

		public int CurrentAmmo {
			get { return _currentAmmo; }
			set { _currentAmmo = value; }
		}

		public IWeapon.AmmoType AmmoTypeDefinition {
			get { return _ammoTypeDefinition; }
			set { _ammoTypeDefinition = value; }
		}

		public float AttackActionTime {
			get { return _attackActionTime; }
			set { _attackActionTime = value; }
		}
	}
}
