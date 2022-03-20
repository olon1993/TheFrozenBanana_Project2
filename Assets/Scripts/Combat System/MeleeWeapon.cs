using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class MeleeWeapon : MonoBehaviour, IMeleeWeapon
    {
        [SerializeField] protected bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        [SerializeField] private Damage _damage;
        [SerializeField] private IWeapon.AmmoType _ammoTypeDefinition;

        [SerializeField] private bool _isLimitedAmmo;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _currentAmmo;

        [SerializeField] protected Transform _pointOfOrigin;
        [SerializeField] protected float _radiusOfInteraction;

        [SerializeField] float _delayToHit = 0f;
        [SerializeField] float _attackActionTime = 0.1f;
        [SerializeField] float _damageForce = 5f;

		[SerializeField] private int _animationLayer;

		// Sound
		[SerializeField] private AudioSource weaponAudioSource;
		[SerializeField] private AudioClip[] weaponSounds;

        private bool _is2D = true;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Start()
        {
            if (_damage == null)
            {
                Debug.LogError("Damage not found on " + gameObject.name);
            }
        }
		
		public void ToggleWeapon(bool on) {

		}

        public void Attack()
        {
            if (_showDebugLog)
            {
                Debug.Log("Attacking with " + name);
            }

            // This should be broken out into a 2d and a 3d version of this mechanic
            if (_is2D)
            {
                Invoke("HandleDamage2D", _delayToHit);
            }
            else
            {
                HandleDamage();
            }
        }

        private void HandleDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(_pointOfOrigin.position, _radiusOfInteraction);
            foreach (Collider collider in colliders)
            {
                IHealth health = collider.GetComponent<IHealth>();
                if (health == null)
                {
                    continue;
                }

                bool isDead = health.TakeDamage(Damage);

                if (_showDebugLog)
                {
                    Debug.Log(gameObject.name + " attacks dealing " + Damage.DamageAmount + " damage to " + collider.gameObject.name + "!");

                    if (isDead == false)
                    {
                        Debug.Log(collider.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);
                    }
                }
            }
        }

        protected virtual void HandleDamage2D()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_pointOfOrigin.position, _radiusOfInteraction);
			if (colliders.Length > 0) {
				HandleSound();
			}
            foreach (Collider2D collider in colliders)
            {
                HandleDamageForce(collider);

                IHealth health = collider.GetComponent<IHealth>();
                if (health == null)
                {
                    continue;
                }

                bool isDead = health.TakeDamage(Damage);

                if (_showDebugLog)
                {
                    Debug.Log(gameObject.name + " attacks dealing " + Damage.DamageAmount + " damage to " + collider.gameObject.name + "!");

                    if (isDead == false)
                    {
                        Debug.Log(collider.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);
                    }
                }
            }
        }

        private void HandleDamageForce(Collider2D collider)
        {
            float damageDirection = transform.position.x < collider.transform.position.x ? 1 : -1;

            ICanBeAffectedByDamageForce objectAffectedByDamageForce = collider.GetComponent<ICanBeAffectedByDamageForce>();

            if (objectAffectedByDamageForce != null)
            {
                objectAffectedByDamageForce.ApplyDamageForce(_damageForce, damageDirection);
            }
        }

		private void HandleSound() {
			if (weaponAudioSource == null) {
				return;
			}
			if (weaponSounds.Length > 1) {
				weaponAudioSource.clip = weaponSounds[Random.Range(0,weaponSounds.Length)];
			}
			weaponAudioSource.Play();
		}

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

        public Damage Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public bool IsLimitedAmmo
        {
            get { return _isLimitedAmmo; }
            set { _isLimitedAmmo = value; }
        }

        public int MaxAmmo
        {
            get { return _maxAmmo; }
            set { _maxAmmo = value; }
        }

        public int CurrentAmmo
        {
            get { return _currentAmmo; }
            set { _currentAmmo = value; }
        }

        public IWeapon.AmmoType AmmoTypeDefinition
        {
            get { return _ammoTypeDefinition; }
            set { _ammoTypeDefinition = value; }
        }

        public Transform PointOfOrigin
        {
            get { return _pointOfOrigin; }
            set { _pointOfOrigin = value; }
        }

        public float AttackRange { get { return _radiusOfInteraction; } }

		public int AnimationLayer {
			get { return _animationLayer; }
			set { _animationLayer = value; }
		}

        public float AttackActionTime
        {
            get { return _attackActionTime; }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_pointOfOrigin.position, _radiusOfInteraction);
        }

    }
}
