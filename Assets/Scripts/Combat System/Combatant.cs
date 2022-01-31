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

    private int _horizontalFacingDirection = 1;

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

        _currentWeapon = transform.GetComponentInChildren<IWeapon>();
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

    private void Update()
    {
        if (IsAttacking)
        {
            CurrentWeapon.Attack();
        }
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public IHealth Health
    {
        get { return _health; }
        set
        {
            if (_health != value)
            {
                _health = value;
            }
        }
    }

    public IList<IWeapon> Weapons
    {
        get { return _weapons; }
        set
        {
            if (_weapons != value)
            {
                _weapons = value;
            }
        }
    }

    public IWeapon CurrentWeapon 
    {
        get { return _currentWeapon; }
        set
        {
            if(_currentWeapon != value)
            {
                _currentWeapon = value;
            }
        }
    }

    public bool IsAttacking { get; set; }

    public int HorizontalFacingDirection 
    {
        get { return _horizontalFacingDirection; }
        set
        {
            if(_horizontalFacingDirection != value && value != 0)
            {
                _horizontalFacingDirection = value;
                if (Mathf.Sign(CurrentWeapon.PointOfOrigin.localPosition.x) != Mathf.Sign(_horizontalFacingDirection))
                {
                    CurrentWeapon.PointOfOrigin.transform.localPosition *= -1;
                }
            }
        }
    }

}
