using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IMeleeWeapon : IWeapon
    {
        float AttackRange { get; }
    }
}
