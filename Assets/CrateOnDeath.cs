using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public class CrateOnDeath : MonoBehaviour, IOnDeath
    {
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void DoThisOnDeath()
        {
            animator.SetTrigger("Destroy");
            GetComponent<PhysicsObject2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public void DestroyMe()
        {
            Destroy(gameObject);
        }
    }
}

