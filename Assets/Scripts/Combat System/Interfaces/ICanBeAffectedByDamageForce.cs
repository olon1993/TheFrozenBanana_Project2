using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public interface ICanBeAffectedByDamageForce
    {
        void ApplyDamageForce(float forceAmount, float direction);
    }
}

