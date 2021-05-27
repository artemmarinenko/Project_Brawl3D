using GameEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ValueWrapper<T> where T : struct
{
    public T Value { get; set; }
    public ValueWrapper(T value) { this.Value = value; }
}

public static class MoveUtility 
{
    public const float LerpTreshHold = 0.001f;

    public static void NoAttackMoveMod(Rigidbody rigidbody,bool IsMoving,float speed)
    {
        if (IsMoving)
        {
            rigidbody.velocity = rigidbody.transform.forward * speed;
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public static void AttackMoveMod(Rigidbody rigidbody, Animator animator, Vector3 direction, bool IsMoving, float speed)
    {
        if (IsMoving)
        { 
            rigidbody.velocity = new Vector3(direction.x, direction.z, direction.y) * speed;
            animator.SetFloat(StringValueHelper.VelocityX, direction.x);
            animator.SetFloat(StringValueHelper.VelocityZ, direction.y);
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }


    public static void Rotate(Transform transform, float angularSpeed, Vector3 direction)
    {
        Vector3 joystickDirection = new Vector3(direction.x, direction.z, direction.y);
        transform.forward = Vector3.Lerp(transform.forward, joystickDirection, angularSpeed * Time.deltaTime);
    }

    public static void RotateForAttack(Transform transform, float angularSpeed, Vector3 direction, ValueWrapper<bool> isRotateEnded)
    {
        if (!isRotateEnded.Value)
        {
            Rotate(transform, angularSpeed, direction);
        }
        else if(isRotateEnded.Value)
        {
            return;
        }

        if (Mathf.Abs(transform.forward.x - direction.x) <= LerpTreshHold && Mathf.Abs(transform.forward.y - direction.z) <= LerpTreshHold && !isRotateEnded.Value)
        {
            isRotateEnded.Value = true;
            transform.forward = new Vector3(direction.x, direction.z, direction.y);
            EventAggregator.Post(transform, new OnRotationBeforeAttackEndedEvent() { });
        }
    }



}
