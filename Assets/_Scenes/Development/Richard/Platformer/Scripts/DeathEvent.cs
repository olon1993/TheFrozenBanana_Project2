using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : MonoBehaviour
{
    void OnDestroy()
    {
        EventBroker.CallEnemyKilled();
    }
}
