using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    bool alreadyInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyInRange) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            alreadyInRange = true;
            EventBroker.CallLevelExitReached();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alreadyInRange = false;
        }
    }
}
