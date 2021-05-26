using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents 
{

    public class OnDragAttackJoystickEvent { };
    public class OnEndDragAttackJoystickEvent { public Vector3 Direction; };
    

    public class OnRotationBeforeAttackEndedEvent { };
    public class AttackEndedEvent { }

    public class PlayerKilledEvent { };
    public class BotKilledEvent { public CharacterType CharacterType; public int Layer; public LayerMask AttackLayer; };

}
