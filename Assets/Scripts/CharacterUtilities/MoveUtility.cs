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

    public static void FireMoveMod(Rigidbody rigidbody,Animator animator, IJoystiсk MoveJoystick, bool IsMoving, float speed)
    {
        if (IsMoving)
        {
            Debug.Log("Second move mod is ON");
            rigidbody.velocity = 1f * new Vector3(MoveJoystick.Direction.x, MoveJoystick.Direction.z, MoveJoystick.Direction.y);
            animator.SetFloat("VelocityX", MoveJoystick.Direction.x);
            animator.SetFloat("VelocityZ", MoveJoystick.Direction.y);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
