using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectOnDeath : MonoBehaviour, IOnDeath
{
    [SerializeField] GameObject particleEffectPrefab;
    public void DoThisOnDeath()
    {
        Instantiate(particleEffectPrefab, transform.position, particleEffectPrefab.transform.rotation);
        gameObject.SetActive(false);
    }
}
