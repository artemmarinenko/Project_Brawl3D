using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum CharacterType {
            Adam,
            Eva
    }
public class Character : MonoBehaviour, IMoveable, IAttackable
{
    [SerializeField] protected float _angularSpeed = 20f;
    [SerializeField] protected float _angularAttackSpeed = 40f;
    [SerializeField] protected float _speedNoAttackMoveMod = 3f;
    [SerializeField] protected float _speedAttackMoveMod = 2f;
    [SerializeField] protected AttackSector _attackSector;
    [SerializeField] protected Slider _hpSlider;
    [SerializeField] protected CharacterType _charType;
    

    protected float _hp = 100;

    protected Rigidbody _rigidBody;
    protected bool _isMoving = false;
    protected bool _isFire = false;
    protected ValueWrapper<bool> _isAttackRotateEnded = new ValueWrapper<bool>(false);

    protected Vector3 _attackDirection;
    protected Animator _animator;
    

    public IWeapon Weapon { get; set; }

    
    
    private void Awake()
    {
        
        EventAggregator.Subscribe<OnRotationBeforeAttackEndedEvent>(OnRotationBeforeAttackEndedHandler);
        EventAggregator.Subscribe<AttackEndedEvent>(AttackEndedHandler);

        _attackSector.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public virtual void MoveControll(Vector3 direction)
    {
        if (Vector3.Magnitude(direction) > 0)
        {
            _isMoving = true;

            if (!_isFire)
            {
                _animator.SetFloat("Speed", 100f);

            }
        }

        else if (direction == Vector3.zero && !_isFire)
        {
            _isMoving = false;
            _animator.SetFloat("Speed", 0);

        }
    }
    public virtual void DOMove(Vector3 direction)
    {
        if (!_isFire)
        {
            MoveUtility.NoAttackMoveMod(_rigidBody, _isMoving, _speedNoAttackMoveMod);
            MoveUtility.Rotate(transform, _angularAttackSpeed, direction);
        }

        else if (_isFire)
        {
            MoveUtility.AttackMoveMod(_rigidBody, _animator, direction, _isMoving, _speedAttackMoveMod);
            // Need to set attack direction to rotate;
            MoveUtility.RotateForAttack(transform, _angularSpeed, _attackDirection, _isAttackRotateEnded);
        }
    }

    public bool ChangeFireStatus(bool attack)
    {
        return _isFire = attack;
    }
    public virtual  void Attack()
    {
        Weapon.Attack();
    }

    public virtual void RecieveDamage(float damage)
    {
        if (_hp - damage >= 0) {

            _hp -= damage;
            _hpSlider.value = _hpSlider.value - (damage / 100);
        }
        else
        {

            Destroy(gameObject);           
        }          
    }

    

    #region EventHandlers
    protected virtual void OnRotationBeforeAttackEndedHandler(object sender, OnRotationBeforeAttackEndedEvent onRotationBeforeAttackEndedEvent)
    {
        if (transform == sender as Transform)
            Attack();
    }
    protected virtual void AttackEndedHandler(object sender, AttackEndedEvent onRotationBeforeAttackEndedEvent)
    {
        if (Weapon == sender as IWeapon)
        {
            //Debug.Log("Weapon from sender attack endded");
            ChangeFireStatus(false);
            _isAttackRotateEnded.Value = false;
            _animator.SetBool("isFire", ChangeFireStatus(false));
        }
    }

    #endregion
}
