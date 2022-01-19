using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combatant : MonoBehaviour, ICombatant
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    private IHealth _health;
    private IList<IWeapon> _weapons;
    private IWeapon _currentWeapon;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    void Awake()
    {
        _health = transform.GetComponent<IHealth>();

        if (_health == null)
        {
            Debug.LogError("IHealth not found on " + gameObject.name);
        }

        _currentWeapon = transform.GetComponent<IWeapon>();
        if (_currentWeapon == null)
        {
            Debug.LogError("IWeapon not found on " + gameObject.name);
        }

        _weapons = new List<IWeapon>();
        if (_weapons == null)
        {
            Debug.LogError("IList<IWeapon> not found on " + gameObject.name);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IList<IWeapon> weapons = transform.GetComponents<IWeapon>().ToList();
        foreach (IWeapon weapon in weapons)
        {
            _weapons.Add(weapon);
        }
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public IHealth Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IList<IWeapon> Weapons { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IWeapon CurrentWeapon { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

}
