using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    bool IsLimitedAmmo { get; set; }
    int MaxAmmo { get; set; }
    int CurrentAmmo { get; set; }
    AmmoType AmmoTypeDefinition { get; set; }

    public enum DamageType
    {
        PHYSICAL, FIRE
    }

    public enum AmmoType
    {
        NONE, MAGIC
    }

    void Attack();

}
