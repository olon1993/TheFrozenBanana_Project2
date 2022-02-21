using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface ICombatant
    {
        IHealth Health { get; set; }
        IList<IWeapon> Weapons { get; set; }
        IWeapon CurrentWeapon { get; set; }
        bool IsAttacking { get; set; }
        int HorizontalFacingDirection { get; set; }
    }
}
