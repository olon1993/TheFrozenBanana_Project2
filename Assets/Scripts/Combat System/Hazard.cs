using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class Hazard : MonoBehaviour
    {
        Damage _damage;
        [SerializeField] float damageForce = 5f;

        private void Awake()
        {
            _damage = GetComponent<Damage>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<IHealth>().TakeDamage(_damage);
                float damageDirection = transform.position.x < other.transform.position.x ? 1 : -1;
                other.GetComponent<ICanBeAffectedByDamageForce>().ApplyDamageForce(damageForce, damageDirection);
            }
        }
    }
}

