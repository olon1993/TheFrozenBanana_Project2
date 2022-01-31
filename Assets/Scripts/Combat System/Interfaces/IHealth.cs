using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    bool TakeDamage(Damage damage);
    int MaxHealth { get; set; }
    int CurrentHealth { get; set; }
}
