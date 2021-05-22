using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public static event Action<Vector3> OnEndDragJoystick;
    
    public static void RaiseOnEndDragJoystick(Vector3 direction)
    {
        OnEndDragJoystick?.Invoke(direction);
    }
  
}
