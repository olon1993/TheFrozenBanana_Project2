using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface IMeleeWeapon : IWeapon
    {
        public float AttackRange { get; }
    }
}
