using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatant 
{
    IHealth Health { get; set; }
    IList<IWeapon> Weapons { get; set; }
    IWeapon CurrentWeapon { get; set; }
    bool IsAttacking { get; set; }
}
