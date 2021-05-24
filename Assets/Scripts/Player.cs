using GameEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    

    [SerializeField] private float _angularSpeed = 20f;
    [SerializeField] private float _angularSpeedAttack = 40f;
    [SerializeField] private float _speedNoFireMoveMod = 3f;
    [SerializeField] private float _speedFireMoveMod = 2f;
    [SerializeField] private GameObject _shootSector;
    [SerializeField] private Blaster _weapon;
    

    private Rigidbody _rigidBody;
    private Animator _animator;

    private bool _isMoving = false;
    private bool _isFire = false;
    private Vector3 _attackDirection;
    private ValueWrapper<bool> _isAttackRotateEnded = new ValueWrapper<bool>(false);


    public IJoystiсk MoveJoystick { get; set; }
    public IJoystiсk ShootJoystick { get; set; }

    

    void Awake()
    {
        EventAggregator.Subscribe<OnDragAttackJoystickEvent>(OnDragAttackJoystickHandler);
        EventAggregator.Subscribe<OnEndDragAttackJoystickEvent>(OnEndDragAttackJoystickHandler);
        EventAggregator.Subscribe<OnRotationBeforeAttackEndedEvent>(OnRotationBeforeAttackEndedHandler);
        EventAggregator.Subscribe<AttackEndedEvent>(AttackEndedHandler);

        
        _shootSector.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

    } 

    
    void Update()
    {

        if (Vector3.Magnitude(MoveJoystick.Direction)>0)
        {
            _isMoving = true;

            if(!_isFire)
            {
                _animator.SetFloat("Speed", 100f);
                MoveUtility.Rotate(transform, _angularSpeedAttack, MoveJoystick.Direction);
            }
        }
        
        else if(MoveJoystick.Direction == Vector3.zero && !_isFire) 
        {
            _isMoving = false;
            _animator.SetFloat("Speed", 0);

        }


        MoveUtility.Rotate(_shootSector.transform, _angularSpeedAttack, ShootJoystick.Direction);

    }

    private void FixedUpdate()
    {
        if (!_isFire)
        {
            MoveUtility.NoAttackMoveMod(_rigidBody, _isMoving, _speedNoFireMoveMod);
        }

        else if (_isFire) {

            MoveUtility.AttackMoveMod(_rigidBody, _animator, MoveJoystick, _isMoving, _speedFireMoveMod);
            MoveUtility.RotateForAttack(transform, _angularSpeed, _attackDirection,_isAttackRotateEnded);
            
        }


    }

 

    IEnumerator WaitForSecondsDuringFire(float seconds)
    {
        _animator.SetBool("isFire", ChangeFireStatus(true));

        yield return new WaitForSeconds(seconds);
        _animator.SetBool("isFire", ChangeFireStatus(false));
    }

    public bool ChangeFireStatus(bool attack)
    {
        return _isFire = attack;
    }


    public void Attack()
    {
        
        _weapon.Attack();
        
    }

 
    #region EventHandlers
    private void OnDragAttackJoystickHandler(object sender, OnDragAttackJoystickEvent onDragAttackJoystickEvent)
    {
        _shootSector.SetActive(true);
    }

    private void OnEndDragAttackJoystickHandler(object sender, OnEndDragAttackJoystickEvent onEndDragAttackJoystickEvent)
    {
        
        _attackDirection = onEndDragAttackJoystickEvent.Direction;
        _shootSector.SetActive(false);
        //StartCoroutine(WaitForSecondsDuringFire(1f));
        _animator.SetBool("isFire", ChangeFireStatus(true));


    }

    private void OnRotationBeforeAttackEndedHandler(object sender, OnRotationBeforeAttackEndedEvent onRotationBeforeAttackEndedEvent)
    {
        Attack();
    }
    private void AttackEndedHandler(object sender, AttackEndedEvent onRotationBeforeAttackEndedEvent)
    {
        Debug.Log("AttackEndede Event");
        ChangeFireStatus(false);
        _isAttackRotateEnded.Value = false;
        _animator.SetBool("isFire", ChangeFireStatus(false));

    }


    #endregion
}

