using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltCombatant : MonoBehaviour
{
    [SerializeField] private bool _showDebugLog = false;

    //**************************************************\\
    //********************* Fields *********************\\
    //**************************************************\\

    private AltHealth _health;
    private IList<IWeapon> _weapons;
    private IWeapon _currentWeapon;

    bool canAttack = true;

    private int _horizontalFacingDirection = 1;

    //**************************************************\\
    //******************** Methods *********************\\
    //**************************************************\\

    void Awake()
    {
        GetDependencies();
    }

    void GetDependencies()
    {
        _health = transform.GetComponent<AltHealth>();

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

    void Start()
    {
        GetWeaponsList();
    }
    void GetWeaponsList()
    {
        IList<IWeapon> weapons = transform.GetComponents<IWeapon>().ToList();
        foreach (IWeapon weapon in weapons)
        {
            _weapons.Add(weapon);
        }
    }

    public void OnAttack()
    {
        if (canAttack)
        {
            _currentWeapon.Attack();
            IsAttacking = true;
            canAttack = false;
            StartCoroutine(AttackTimer(_currentWeapon.AttackActionTime));
        }
    }

    IEnumerator AttackTimer(float time)
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
        IsAttacking = false;

        if(_showDebugLog)
            Debug.Log("Reset canAttack" + canAttack);
    }

    void UpdateHorizontalFacingDirection(int value)
    {
        if (_horizontalFacingDirection != value && value != 0)
        {
            _horizontalFacingDirection = value;
            if (Mathf.Sign(CurrentWeapon.PointOfOrigin.localPosition.x) != Mathf.Sign(_horizontalFacingDirection))
            {
                CurrentWeapon.PointOfOrigin.transform.localPosition *= -1;
            }
        }
    }

    //**************************************************\\
    //******************* Properties *******************\\
    //**************************************************\\

    public AltHealth Health
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
            if (_currentWeapon != value)
            {
                _currentWeapon = value;
            }
        }
    }

    public bool IsAttacking { get; set; }

    public bool CanAttack { get; }

    public int HorizontalFacingDirection
    {
        get { return _horizontalFacingDirection; }
        set
        {
            UpdateHorizontalFacingDirection(value);
        }
    }
}