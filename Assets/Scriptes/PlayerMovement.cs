using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Animator _animator;
    private bool _isMoving = false;
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _animator.SetFloat("Speed", 1f);
            _isMoving = true;

        }

        

        else
        {
            _isMoving = false;
            _animator.SetFloat("Speed", 0f);
        }

        Rotate();

        
        
    }

    private void FixedUpdate()
    {
        if (_isMoving)
            _rigidBody.velocity = transform.forward*3;
        else {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
             }


    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * 300);
    }
}

