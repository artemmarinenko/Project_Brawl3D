﻿using GameEvents;
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

    public static void AttackMoveMod(Rigidbody rigidbody,Animator animator, IJoystiсk MoveJoystick, bool IsMoving, float speed)
    {
        if (IsMoving)
        {
            
            rigidbody.velocity = new Vector3(MoveJoystick.Direction.x, MoveJoystick.Direction.z, MoveJoystick.Direction.y)*speed;
            animator.SetFloat("VelocityX", MoveJoystick.Direction.x);
            animator.SetFloat("VelocityZ", MoveJoystick.Direction.y);
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

        if (Mathf.Abs(transform.forward.x - direction.x) <= 0.001f && Mathf.Abs(transform.forward.y - direction.z) <= 0.001f && !isRotateEnded.Value)
        {
            isRotateEnded.Value = true;
            transform.forward = new Vector3(direction.x, direction.z, direction.y);
            EventAggregator.Post(transform.gameObject, new OnRotationBeforeAttackEndedEvent() { });
        }
  
    }
}
