using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Combatant : MonoBehaviour, ICombatant
    {
        // [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        private IInputManager _inputManager;

        private IHealth _health;
        private IList<IWeapon> _weapons;
        private IWeapon _currentWeapon;

        private int _horizontalFacingDirection = 1;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        void Awake()
        {
            _inputManager = GetComponent<IInputManager>();
            if (_inputManager == null)
            {
                Debug.Log("No Input Manager found on " + name);
            }

            _health = transform.GetComponent<IHealth>();
            if (_health == null)
            {
                Debug.LogError("IHealth not found on " + gameObject.name);
            }

            _currentWeapon = transform.GetComponentInChildren<IWeapon>();
            if (_currentWeapon == null)
            {
                Debug.LogError("IWeapon not found on " + gameObject.name);
            }

            _weapons = new List<IWeapon>();
            if (_weapons == null)
            {
                Debug.LogError("IList<IWeapon> not found on " + gameObject.name);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            IList<IWeapon> weapons = transform.GetComponentsInChildren<IWeapon>().ToList();
            foreach (IWeapon weapon in weapons)
            {
                _weapons.Add(weapon);
            }
        }

		private void Update() {
			IsAttacking = _inputManager.IsAttack;

			if (IsAttacking) {
				CurrentWeapon.Attack();
			}

			CheckWeaponToggle();
		}

		private void CheckWeaponToggle() { 

			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				// Weapon 1 - Unarmed
				try {
					CurrentWeapon.ToggleWeapon(false);
					CurrentWeapon = _weapons[0];
					CurrentWeapon.ToggleWeapon(true);
				} catch (ArgumentOutOfRangeException aoore) {
					Debug.Log("Weapon 0 not found: " + aoore);
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				// Weapon 2 - Fireball Cannon
				try {
					CurrentWeapon.ToggleWeapon(false);
					CurrentWeapon = _weapons[1];
					CurrentWeapon.ToggleWeapon(true);
				} catch (ArgumentOutOfRangeException aoore) {
					Debug.Log("Weapon 1 not found: " + aoore);
				}
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				// Weapon 3 - Not yet added
				try {
					CurrentWeapon.ToggleWeapon(false);
					CurrentWeapon = _weapons[2];
					CurrentWeapon.ToggleWeapon(true);
				} catch (ArgumentOutOfRangeException aoore) {
					Debug.Log("Weapon 2 not found: " + aoore);
				}
			}
		}

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public IHealth Health
        {
            get { return _health; }
            set
            {
                if (_health != value)
                {
                    _health = value;
                }
            }
        }

        public IList<IWeapon> Weapons
        {
            get { return _weapons; }
            set
            {
                if (_weapons != value)
                {
                    _weapons = value;
                }
            }
        }

        public IWeapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                if (_currentWeapon != value)
                {
                    _currentWeapon = value;
                }
            }
        }

        public bool IsAttacking { get; set; }

        public int HorizontalFacingDirection
        {
            get { return _horizontalFacingDirection; }
            set
            {
                if (_horizontalFacingDirection != value && value != 0)
                {
                    _horizontalFacingDirection = value;
                    if (Mathf.Sign(CurrentWeapon.PointOfOrigin.localPosition.x) != Mathf.Sign(_horizontalFacingDirection))
                    {
                        CurrentWeapon.PointOfOrigin.transform.localPosition *= -1;
                    }
                }
            }
        }

    }
}
