using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectedByDamageForce
{
    IEnumerator ApplyDamageForce(float forceAmount, float direction);  
}
