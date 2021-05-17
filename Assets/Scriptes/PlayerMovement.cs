﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    

    [SerializeField]private float _angularSpeed = 20f;
    [SerializeField] private float _speed = 3f;
    private Rigidbody _rigidBody;
    private Animator _animator;

    public Joystick Move { get; set; }

    private bool _isMoving = false;
    
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
       
    }

    
    void Update()
    {
        

        if (Vector3.Magnitude(Move._direction)>0)
        {
            _animator.SetFloat("Speed", 1f);
            _isMoving = true;
            Rotate();

        }
        

        else if(Move._direction == Vector3.zero) 
        {
            _isMoving = false;
            _animator.SetFloat("Speed", 0f);

        }


    }

    private void FixedUpdate()
    {
        if (_isMoving)
            _rigidBody.velocity = transform.forward* _speed;
        else {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
             }


    }

    private void Rotate()
    {
       transform.forward = Vector3.Lerp(transform.forward,new Vector3(Move._direction.x, Move._direction.z, Move._direction.y), _angularSpeed * Time.deltaTime);
    }
}

