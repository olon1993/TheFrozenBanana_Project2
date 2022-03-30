using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheFrozenBanana;

namespace TheFrozenBanana {

	public class RangedWeapon : MonoBehaviour, IRangedWeapon {

		// From IRangedWeapon
		[SerializeField] private GameObject _projectile;
		[SerializeField] private Transform _target;
		[SerializeField] private Transform _aimTool;
		[SerializeField] private Renderer _weaponObject;
		[SerializeField] private Vector3 _localScale;
		[SerializeField] private bool _canAim;
		[SerializeField] private float _maxAngle;
		[SerializeField] private float _projectileKillTime;
		[SerializeField] private Camera _cam;
		private float _dirSign;

		// From IWeapon
		[SerializeField] private bool _isLimitedAmmo;
		[SerializeField] private int _maxAmmo;
		[SerializeField] private int _currentAmmo;
		[SerializeField] private IWeapon.AmmoType _ammoTypeDefinition;
		[SerializeField] private Transform _pointOfOrigin;
		[SerializeField] private float _attackActionTime;
		[SerializeField] private int _animationLayer;

		// Extras
		[SerializeField] private Transform startVector;
		[SerializeField] private int spawnProjectileAnimFrame;
		[SerializeField] private int totalFiringAnimFrames;
		[SerializeField] private AudioSource weaponAudioSource;
		[SerializeField] private bool enemyWeapon;
		[SerializeField] private string animationName;
		[SerializeField] private string ownerTag;
		private bool firing;
		private Animator weaponAC;
		private ILocomotion _locomotion;

		private void Awake() {
			firing = false;
			weaponAC = GetComponentInChildren<Animator>();
			_locomotion = gameObject.GetComponentInParent<ILocomotion>();
			if (_locomotion == null) {
				Debug.LogError(gameObject.name + " has a ranged weapon that cannot find the owner's locomotion!");
			}
			if (_canAim && !enemyWeapon) {
				cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
				if (cam == null) {
					Debug.LogError(gameObject.name + "has a weapon that can aim but no camera can be found to aim with!");
				}
			} else if (enemyWeapon) {
				StartCoroutine(FindPlayer());
			}
			if (_canAim && _aimTool == null && !enemyWeapon) {
				Debug.LogError(gameObject.name + "has a weapon that can aim but no target finder!");
			}
			if (_localScale == Vector3.zero) {
				_localScale = new Vector3(1,1,1);
			}
			if (projectileKillTime == 0) {
				projectileKillTime = 3f;
			}
			ToggleWeapon(false);
		}

		private void OnEnable() {
			if (enemyWeapon) {
				StartCoroutine(FindPlayer());
			}
		}

		private IEnumerator FindPlayer() {
			while (aimTool == null) {
				yield return new WaitForSeconds(0.1f);
				try {
					aimTool = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
				} catch (NullReferenceException nre) {
					Debug.Log("Enemy has not (yet) found the player: " + nre);
				}
			}
		}

		public void Attack() {
			if (firing) {
				return;
			}
			firing = true;
			if (weaponAC != null) {
				weaponAC.Play(animationName);
			}
			StartCoroutine(DelayedFiring());
		//	SpawnProjectile();
		}

		private void Update() {
			if (!enemyWeapon) {
				UpdateTargetLocation();
			}
			UpdateRotation();
			
			if (enemyWeapon) {
				target = aimTool;
			}
		}

		// This is where the weapon picks up where to aim at.
		private void UpdateTargetLocation() {
			if (canAim && aimTool != null) {
				Vector2 mousePos = Input.mousePosition;
				Vector3 pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
				aimTool.position = pos;
				if (Vector2.Distance(gameObject.transform.position, aimTool.position) < 1) {
					aimTool.position = gameObject.transform.position + new Vector3(_locomotion.HorizontalLook, 0, 0);
				}
			}
		}

		// This is where the weapon's renderer is rotated to point at where it's aiming.
		private void UpdateRotation() {
			// need 2 vectors: the base vector (which is the look vector of the player)
			// and the vector to the target position

			dirSign = 1;
			if (_locomotion.IsWallSliding) {
				dirSign = -1;
			}
			Vector2 vBase = new Vector2(_locomotion.HorizontalLook * Mathf.Sign(dirSign), 0);
			float angle = 0;
			if (canAim && aimTool != null) {
				Vector2 vDir = aimTool.position - gameObject.transform.position;
				angle = Vector2.Angle(vBase, vDir) * _locomotion.HorizontalLook * Mathf.Sign(dirSign);
				if (aimTool.position.y < gameObject.transform.position.y) {
					angle = -angle;
				}
				if (Mathf.Abs(angle) > maxAngle) {
					angle = maxAngle * Mathf.Sign(angle);
				}
			}
			weaponObject.transform.localScale = new Vector3(localScale.x * Mathf.Sign(dirSign), localScale.y, localScale.z);
			weaponObject.transform.rotation = Quaternion.Euler(0, 0, angle);
		}

		// The delayed firing routine is a test to see if the projectile spawning can be timed with the animation
		private IEnumerator DelayedFiring() {
			float timeA = AttackActionTime * ((float) spawnProjectileAnimFrame / (float) totalFiringAnimFrames);
			float timeB = AttackActionTime - timeA; 
			yield return new WaitForSeconds(timeA);
			SpawnProjectile();
			yield return new WaitForSeconds(timeB);
			firing = false;
		}

		// This is where the weapon actually shoots
		private void SpawnProjectile() { 
			if (weaponAudioSource != null) {
				weaponAudioSource.Play();
			}
			Quaternion rot = weaponObject.transform.rotation;
			if (_locomotion.HorizontalLook < 0) {
				rot *= Quaternion.Euler(0, 0, 180);
			}
			if (dirSign < 0) {
				rot *= Quaternion.Euler(0, 0, 180);
			}
			Vector3 fireTowards = target.position;
			GameObject proj = Instantiate(projectile, PointOfOrigin.position, Quaternion.identity, null) as GameObject;
			Vector3 startDirection = gameObject.transform.position;
			if (startVector != null) {
				startDirection = startVector.position;
			}
			proj.GetComponent<IProjectile>().Setup(startDirection, fireTowards, rot, ownerTag);
			Destroy(proj, projectileKillTime);
		}

		public void ToggleWeapon(bool on) {
			if (_weaponObject != null) {
				_weaponObject.enabled = on;
			}
		}


		// Getters and Setters from Interfaces
		// IRangedWeapon
		public GameObject projectile {
			get { return _projectile; }
		}
		
		public Transform aimTool {
			get { return _aimTool; }
			set { _aimTool = value; }
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

		public float maxAngle {
			get { return _maxAngle; }
			set { _maxAngle = value; }
		}

		public float projectileKillTime {
			get { return _projectileKillTime; }
			set { _projectileKillTime = value; }
		}

		public Camera cam {
			get { return _cam; }
			set { _cam = value; }
		}

		public float dirSign {
			get { return _dirSign; }
			set { _dirSign = value; }
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

		public int AnimationLayer {
			get { return _animationLayer; }
			set { _animationLayer = value; }
		}
	}
}
