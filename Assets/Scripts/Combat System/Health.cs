using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheFrozenBanana
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealth;
		private bool _isDead;
        private bool _isHurt;

        [SerializeField] Image HealthBar;
        [SerializeField] AudioClip HurtSoundEffect;
        [SerializeField] AudioClip DieSoundEffect;
        [SerializeField] List<Damage> DamageMultipliers;

        [SerializeField] GameObject _enableOnDie;
        [SerializeField] GameObject _disableOnDie;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        void Update()
        {
            _isHurt = false;
        }

        // Returns true if the Die method is called
        public bool TakeDamage(Damage damage)
        {
			if (_isDead) {
				return true;
			}
            CurrentHealth -= damage.DamageAmount;
            _isHurt = true;

            if (HealthBar != null)
            {
                HealthBar.fillAmount = (float)CurrentHealth / MaxHealth;
            }

            if (CurrentHealth <= 0)
            {
                Die();
                return true;
            }
            else if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
            else
            {
                AudioEvents.CallPlaySoundClip(HurtSoundEffect);
            }

            return false;
        }

		public void AddHealth(int hp) {
			CurrentHealth += hp;
			if (CurrentHealth > MaxHealth) {
				CurrentHealth = MaxHealth;
			}
		}

        private void Die()
        {
            _isDead = true;
            // Log
            if (_showDebugLog)
            {
                Debug.Log(gameObject.name + " has died!");
            }

            // Die sound
            if (DieSoundEffect != null)
            {
                AudioEvents.CallPlaySoundClip(DieSoundEffect);
            }

            // Enable
            if (_enableOnDie != null)
            {
                _enableOnDie.SetActive(true);
            }

            // Disable
            if (_disableOnDie != null)
            {
                _disableOnDie.SetActive(false);
            }

            // Destroy
            if (gameObject.CompareTag("Player"))
            {
                EventBroker.CallPlayerDeath();
                gameObject.SetActive(false);
            }
            else
            {
                float delay = DieSoundEffect == null ? 0 : DieSoundEffect.length;
                Destroy(gameObject, delay);
            }
        }

        //**************************************************\\
        //******************* Properties *******************\\
        //**************************************************\\

		public bool IsDead { get {return _isDead; } }
        public bool IsHurt { get { return _isHurt; } }

        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        public int CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }
    }
}
