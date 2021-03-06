using GameEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    public IJoystiсk MoveJoystick { get; set; }
    public IJoystiсk ShootJoystick { get; set; }

    private void OnDestroy()
    {
        EventAggregator.UnSubscribe<OnDragAttackJoystickEvent>(OnDragAttackJoystickHandler);
        EventAggregator.UnSubscribe<OnEndDragAttackJoystickEvent>(OnEndDragAttackJoystickHandler);
        EventAggregator.UnSubscribe<OnRotationBeforeAttackEndedEvent>(OnRotationBeforeAttackEndedHandler);
        EventAggregator.UnSubscribe<AttackEndedEvent>(AttackEndedHandler);
        EventAggregator.Post(null, new PlayerKilledEvent());
    }

    void Awake()
    {
        EventAggregator.Subscribe<OnDragAttackJoystickEvent>(OnDragAttackJoystickHandler);
        EventAggregator.Subscribe<OnEndDragAttackJoystickEvent>(OnEndDragAttackJoystickHandler);
        EventAggregator.Subscribe<OnRotationBeforeAttackEndedEvent>(OnRotationBeforeAttackEndedHandler);
        EventAggregator.Subscribe<AttackEndedEvent>(AttackEndedHandler);

        _attackSector.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        
    }

 
    void Update()
    {
        PlayerMoveControll(MoveJoystick.Direction);

        ShootingSeсtorControll(_attackSector.transform, _angularAttackSpeed, ShootJoystick.Direction);
    }

    private void FixedUpdate()
    {
        PlayerDOMove(MoveJoystick.Direction);
    }

    private void PlayerDOMove(Vector3 direction)
    {
        DOMove(direction);
    }  

    private void PlayerMoveControll(Vector3 direction)
    {
        MoveControll(direction);
    }

    private void ShootingSeсtorControll(Transform transform, float angularAttackSpeed, Vector3 attackJoysticDirection )
    {
        MoveUtility.Rotate(transform, angularAttackSpeed, attackJoysticDirection);
    }


    #region EventHandlers
    private void OnDragAttackJoystickHandler(object sender, OnDragAttackJoystickEvent onDragAttackJoystickEvent)
    {
        _attackSector.gameObject.SetActive(true);
    }

    private void OnEndDragAttackJoystickHandler(object sender, OnEndDragAttackJoystickEvent onEndDragAttackJoystickEvent)
    {
        _attackDirection = onEndDragAttackJoystickEvent.Direction;
        _attackSector.gameObject.SetActive(false);
        _animator.SetBool(StringValueHelper.IsFire, ChangeFireStatus(true));
    }
    #endregion

}

