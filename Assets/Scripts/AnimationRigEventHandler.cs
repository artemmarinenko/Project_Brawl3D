using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRigEventHandler: MonoBehaviour
{
    public static event Action AnimationEvent;
    
    public void RaiseOnFireEndEvent()
    {
        AnimationEvent?.Invoke();
    }
  
}
