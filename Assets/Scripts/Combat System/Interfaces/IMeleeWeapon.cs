using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeWeapon : IWeapon
{
    public float AttackRange { get; }
}
