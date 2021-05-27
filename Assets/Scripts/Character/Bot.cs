using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class Bot : Character
{
    private NavMeshAgent _agent;
    
    private Transform _player;

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layermask;
    [SerializeField] private LayerMask _crystals;

    private float _walkPointRange = 2f;
    private bool _playerInSightRange = false;

    private Vector3 _lastPlayerPosition;

    public void  SetPlayer(Transform playerTransform)
    {
        _player = playerTransform;
    }

    public void SetLayerMask(LayerMask layerMask)
    {
        _layermask = layerMask;
    }

    private void OnDestroy()
    {
        EventAggregator.UnSubscribe<OnRotationBeforeAttackEndedEvent>(OnRotationBeforeAttackEndedHandler);
        EventAggregator.UnSubscribe<AttackEndedEvent>(AttackEndedHandler);

        EventAggregator.Post(null, new BotKilledEvent() { CharacterType = _charType, Layer = gameObject.layer, AttackLayer = _layermask});
    }
    void Awake()
    {
        EventAggregator.Subscribe<OnRotationBeforeAttackEndedEvent>(OnRotationBeforeAttackEndedHandler);
        EventAggregator.Subscribe<AttackEndedEvent>(AttackEndedHandler);
        _agent = GetComponent<NavMeshAgent>();
        _lastPlayerPosition = transform.position;

        _attackSector.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Collider[] crystals = Physics.OverlapSphere(transform.position, 50, _crystals);

        if (crystals.Length > 0)
        {
            _agent.SetDestination(FindNearestCrystal(crystals).position);
            _animator.SetFloat(StringValueHelper.Speed, 100);
            if (_player != null) {
                if ((Vector3.Distance(transform.position, _player.transform.position) < 4) && !_isFire)
                {
                    Attack();
                    ChangeFireStatus(true);
                }
            }

        }
        
        else {
            SearchAndAttackPlayer();
        }      
    }
    private void FixedUpdate()
    {
        if(_player != null)
            DOMove((transform.position - _player.position).normalized);
    }

    private Transform FindNearestCrystal(Collider[] colliders)
    {
        return colliders.OrderBy(t => Vector3.Distance(transform.position, t.gameObject.transform.position)).FirstOrDefault().gameObject.transform;
    }

    private void SearchAndAttackPlayer() {
        _playerInSightRange = Physics.CheckSphere(transform.position, 5, _layermask);

        if (_playerInSightRange && _player != null)
        {
            _lastPlayerPosition = _player.position;
        }

        MoveControll(_lastPlayerPosition);
        if (_agent.remainingDistance >= 3.5 && _agent.remainingDistance <= 4)
        {
            if (!_isFire)
            {
                Attack();
                ChangeFireStatus(true);
            }
        }

        if (_agent.remainingDistance >= 0.5 && _agent.remainingDistance <= 1)
        {
            if (!_isFire)
            {
                Attack();
                ChangeFireStatus(true);
            }
        }
        if (_agent.remainingDistance <= 1)
        {
            _agent.isStopped = true;
            _animator.SetFloat(StringValueHelper.Speed, 0);
        }
    }

    public override void MoveControll(Vector3 position) {

        if (!(position.x == transform.position.x && position.z == transform.position.z)) {
            
            _agent.SetDestination(position);
            _agent.isStopped = false;
            _animator.SetFloat(StringValueHelper.Speed, 100);
            
        }
        else 
        {          
            _animator.SetFloat(StringValueHelper.Speed, 0);
        }    
    }

    public override void DOMove(Vector3 direction)
    {
        if (!_isFire)
        {
            _agent.speed = _speedNoAttackMoveMod;
        }

        else if (_isFire)
        {
            MoveUtility.AttackMoveMod(_rigidBody, _animator, direction, _isMoving, _speedAttackMoveMod);
            _agent.speed = _speedAttackMoveMod;
            MoveUtility.RotateForAttack(transform, _angularSpeed, _attackDirection, _isAttackRotateEnded);
        }
    }

    protected override void AttackEndedHandler(object sender, AttackEndedEvent onRotationBeforeAttackEndedEvent)
    {
        if (Weapon == sender as IWeapon)
        {
            ChangeFireStatus(false);
            _isAttackRotateEnded.Value = false;
            _animator.SetBool(StringValueHelper.IsFire, ChangeFireStatus(false));
        }
    }

}
 