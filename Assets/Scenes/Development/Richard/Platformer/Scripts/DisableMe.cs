using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMe : MonoBehaviour
{
    public void TurnMeOff()
    {
        gameObject.SetActive(false);
    }
}
