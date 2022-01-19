using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    // Dependencies
    [SerializeField] private Damage _damage;
    [SerializeField] private IWeapon.AmmoType _ammoTypeDefinition;

    [SerializeField] private bool _isLimitedAmmo;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _currentAmmo;

    [SerializeField] private Transform _pointOfInteraction;
    [SerializeField] private float _radiusOfInteraction;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    private void Start()
    {
        if(_damage == null)
        {
            Debug.LogError("Damage not found on " + gameObject.name);
        }
    }

    public void Attack()
    {
        if (_showDebugLog)
        {
            Debug.Log("Attacking with " + name);
        }

        Collider[] colliders = Physics.OverlapSphere(_pointOfInteraction.position, _radiusOfInteraction);
        foreach (Collider collider in colliders)
        {
            IHealth health = collider.GetComponent<IHealth>();
            if (health == null)
            {
                continue;
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
        }
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_pointOfInteraction.position, _radiusOfInteraction);
    }

}
