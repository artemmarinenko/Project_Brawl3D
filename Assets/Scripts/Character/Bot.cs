using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private NavMeshAgent _agent;
    
    private Transform _player;
    //public Transform Player { get { return _player; } set { _player = Player;  } }

    [SerializeField] private Camera _camera;


    private float walkPointRange = 2f;
    private Vector3 walkPoint;
    [SerializeField ]LayerMask layermask;


    private bool _walkPointSearched;
    private bool _playerInSightRange = false;

    Vector3 lastPlayerPosition;

    public void  SetPlayer(Transform playerTransform)
    {
        _player = playerTransform;
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
        _playerInSightRange = Physics.CheckSphere(transform.position, 7, layermask);
        
        
        if (_playerInSightRange) {
            //Attack();
            lastPlayerPosition = _player.position;
            }
        
        MoveControll(lastPlayerPosition);
        if(_agent.remainingDistance >= 3.5 && _agent.remainingDistance <= 4) { 
            if (!_isFire) { 
                Attack();
                ChangeFireStatus(true); }
        }
        if(_agent.remainingDistance<= 1)
        {
            _agent.isStopped = true;
            _animator.SetFloat("Speed", 0);
        }

    }
    private void FixedUpdate()
    {
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
 