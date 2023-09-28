using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager
{
    private static bool isMoving = false;


    public static bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

}
