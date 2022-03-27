using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Hazard : MonoBehaviour
    {
        Damage _damage;

        private void Awake()
        {
            _damage = GetComponent<Damage>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            IHealth otherHealth = other.GetComponent<IHealth>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(_damage);
            }   

            HandleDamageForce(other);
        }

        private void HandleDamageForce(Collider2D other)
        {
            var affectedByDamageForce = other.GetComponent<ICanBeAffectedByDamageForce>();
            if (affectedByDamageForce != null)
            {
                float damageDirection = transform.position.x < other.transform.position.x ? 1 : -1;
                affectedByDamageForce.ApplyDamageForce(_damage.DamageForce, damageDirection);
            }
        }
    }
}

