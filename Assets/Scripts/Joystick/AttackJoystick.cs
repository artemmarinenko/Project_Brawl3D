using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackJoystick : Joystick, IDragHandler
{
    public override void OnDrag(PointerEventData eventData)
    {
        EventAggregator.Post(this, new OnDragAttackJoystickEvent());
        ThumbleManipulation(eventData);
    }

}
