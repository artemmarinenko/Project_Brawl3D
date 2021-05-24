using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents 
{

    public class OnDragAttackJoystickEvent { };
    public class OnEndDragAttackJoystickEvent { public Vector3 Direction; };
    

    public class OnRotationBeforeAttackEndedEvent { };
    public class AttackEndedEvent { }

}
