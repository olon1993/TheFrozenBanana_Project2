using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltHealth : Health
{
    AltLocomotion2D locomotion2D;
    [SerializeField] float damageForce = 500;

    [SerializeField] float invincibleTime = 2f;
    bool invincible;

    private void Awake()
    {
        locomotion2D = GetComponent<AltLocomotion2D>();
    }
    public bool TakeDamage(Damage damage, float direction)
    {
        Debug.Log("TakingDamage: " + gameObject.name);
        if (invincible)
        {
            Debug.Log("You can't touch me! : " + gameObject.name + " is invincible!");
            return false;
        }

        StartCoroutine(InvincibleCountdown());

        if (locomotion2D != null)
            locomotion2D.ApplyDamageForce(damageForce, direction, invincibleTime);

        return base.TakeDamage(damage);
    }

    IEnumerator InvincibleCountdown()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }
}
