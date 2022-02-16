using UnityEngine;

namespace TheFrozenBanana
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private bool _showDebugLog = false;

        //**************************************************\\
        //********************* Fields *********************\\
        //**************************************************\\

        // Dependencies
        [SerializeField] public Damage Damage;
        private Rigidbody _rigidbody;

        [SerializeField] int DamageAmount;
        [SerializeField] IWeapon.DamageType DamageTypeDefinition;
        [SerializeField] public float ProjectileSpeed;

        //**************************************************\\
        //******************** Methods *********************\\
        //**************************************************\\

        private void Awake()
        {
            _rigidbody = transform.GetComponent<Rigidbody>();

            if (_rigidbody == null)
            {
                Debug.LogError("No Rigidbody found on " + gameObject.name);
            }

            Damage = new Damage();
        }

        // Start is called before the first frame update
        void Start()
        {
            Damage.DamageAmount = DamageAmount;
            Damage.DamageType = DamageTypeDefinition;

            _rigidbody.velocity = gameObject.transform.forward * ProjectileSpeed;
        }

        private void OnTriggerEnter(Collider collider)
        {
            IHealth health = collider.GetComponent<IHealth>();
            if (health == null)
            {
                Destroy(gameObject);
                return;
            }

            if (_showDebugLog)
            {
                Debug.Log(gameObject.name + " attacks dealing " + Damage.DamageAmount + " damage to " + collider.gameObject.name + "!");
            }

            bool isDead = health.TakeDamage(Damage);

            if (_showDebugLog)
            {
                if (isDead == false)
                {
                    Debug.Log(collider.gameObject.name + " health = " + health.CurrentHealth + " / " + health.MaxHealth);
                }
            }

            Destroy(gameObject);
        }
    }
}
