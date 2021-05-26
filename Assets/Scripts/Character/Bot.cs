using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Bot : Character
{
    private NavMeshAgent _agent;
    
    private Transform _player;

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layermask;

    private float walkPointRange = 2f;
    private Vector3 walkPoint;
    


    private bool _walkPointSearched;
    private bool _playerInSightRange = false;

    Vector3 lastPlayerPosition;

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
        lastPlayerPosition = transform.position;

        _attackSector.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerInSightRange = Physics.CheckSphere(transform.position, 5, _layermask);
        
        
        if (_playerInSightRange && _player != null) {
            
                lastPlayerPosition = _player.position;
                
            }
        
        MoveControll(lastPlayerPosition);
        if(_agent.remainingDistance >= 3.5 && _agent.remainingDistance <= 4) { 
            if (!_isFire) { 
                Attack();
                ChangeFireStatus(true); }
        }

        if (_agent.remainingDistance >= 0.5 && _agent.remainingDistance <= 1)
        {
            if (!_isFire)
            {
                Attack();
                ChangeFireStatus(true);
            }
        }
        if (_agent.remainingDistance<= 1)
        {
            _agent.isStopped = true;
            _animator.SetFloat("Speed", 0);
        }

    }
    private void FixedUpdate()
    {
        if(_player != null)
            DOMove((transform.position - _player.position).normalized);
    }


    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        _walkPointSearched = true;
    }

    

    protected override void AttackEndedHandler(object sender, AttackEndedEvent onRotationBeforeAttackEndedEvent)
    {
        if (Weapon == sender as IWeapon)
        {
            //Debug.Log("Weapon from sender attack endded");
            ChangeFireStatus(false);
            _isAttackRotateEnded.Value = false;
            _animator.SetBool("isFire", ChangeFireStatus(false));
        }
    }

    public override void MoveControll(Vector3 position) {

        if (!(position.x == transform.position.x && position.z == transform.position.z)) {
            
            _agent.SetDestination(position);
            _agent.isStopped = false;
            _animator.SetFloat("Speed", 100);
            
        }
        else 
        {
            
            _animator.SetFloat("Speed", 0);
        }
        
        
    }

    public override void DOMove(Vector3 direction)
    {
        if (!_isFire)
        {
            _agent.speed = _speedNoAttackMoveMod;
           // MoveUtility.NoAttackMoveMod(_rigidBody, _isMoving, _speedNoAttackMoveMod);
           // MoveUtility.Rotate(transform, _angularAttackSpeed, direction);
        }

        else if (_isFire)
        {
            MoveUtility.AttackMoveMod(_rigidBody, _animator, direction, _isMoving, _speedAttackMoveMod);
            _agent.speed = _speedAttackMoveMod;
            // Need to set attack direction to rotate;
            MoveUtility.RotateForAttack(transform, _angularSpeed, _attackDirection, _isAttackRotateEnded);
        }
    }

}
 