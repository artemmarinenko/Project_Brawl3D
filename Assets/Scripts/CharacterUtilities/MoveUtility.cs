using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveUtility 
{
    public static void NoFireMoveMod(Rigidbody rigidbody,bool IsMoving,float speed)
    {
        if (IsMoving)
        {
            Debug.Log("First move mod is ON");
            rigidbody.velocity = rigidbody.transform.forward * speed;
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public static void FireMoveMod(Rigidbody rigidbody,Animator animator,Joystick MoveJoystick, bool IsMoving, float speed)
    {
        if (IsMoving)
        {
            Debug.Log("Second move mod is ON");
            rigidbody.velocity = 1f * new Vector3(MoveJoystick.direction.x, MoveJoystick.direction.z, MoveJoystick.direction.y);
            animator.SetFloat("VelocityX", MoveJoystick.direction.x);
            animator.SetFloat("VelocityZ", MoveJoystick.direction.y);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
