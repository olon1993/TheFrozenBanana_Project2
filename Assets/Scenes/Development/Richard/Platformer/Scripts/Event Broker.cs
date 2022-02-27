
using System;
using UnityEngine;

public class EventBroker
{
    public static event Action LevelExitReached;

    public static void CallLevelExitReached()
    {
        LevelExitReached?.Invoke();
    }

    public static event Action ShipPartFound;

    public static void CallShipPartFound()
    {
        ShipPartFound?.Invoke();
    }

    public static event Action AllShipPartsFound;

    public static void CallAllShipPartsFound()
    {
        AllShipPartsFound?.Invoke();
    }

    public static event Action LevelCompleted;

    public static void CallLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }
}
