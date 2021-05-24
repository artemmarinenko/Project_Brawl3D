using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackJoystick : Joystick, IDragHandler, IEndDragHandler
{
    public override void OnDrag(PointerEventData eventData)
    {
        ThumbleManipulation(eventData);
        EventAggregator.Post(this, new OnDragAttackJoystickEvent());
        
    }

    public override void OnEndDrag(PointerEventData eventData) {
        EventAggregator.Post(this, new OnEndDragAttackJoystickEvent() { Direction = Direction});
        _thumble.transform.position = _startThumplePosition;
        Direction = Vector3.zero;
    }
    
}
