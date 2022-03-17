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
            Debug.Log(other.name + " collided with " + name);
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<IHealth>().TakeDamage(_damage);
                float damageDirection = transform.position.x < other.transform.position.x ? 1 : -1;
                other.GetComponent<ICanBeAffectedByDamageForce>().ApplyDamageForce(5, damageDirection);
                Debug.Log(name + " did damage to " + other.name);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject.name + " collided with " + name);
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<IHealth>().TakeDamage(_damage);
                float damageDirection = collision.GetContact(0).point.x < collision.transform.position.x ? 1 : -1;
                collision.gameObject.GetComponent<ICanBeAffectedByDamageForce>().ApplyDamageForce(5, damageDirection);
                Debug.Log(name + " did damage to " + collision.gameObject.name);
            }
        }
    }
}

