using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    

    [SerializeField] private float _angularSpeed = 20f;
    [SerializeField] private float _speedNoFireMoveMod = 3f;
    [SerializeField] private float _speedFireMoveMod = 2f;
    [SerializeField] private GameObject _shootSector;
    
    //[SerializeField] private Button _shootButton;
    

    private Rigidbody _rigidBody;
    private Animator _animator;
    

    bool isFire = false;

    public IJoystiсk MoveJoystick { get; set; }
    public IJoystiсk ShootJoystick { get; set; }

    private bool _isMoving = false;
    
    void Awake()
    {
        EventAggregator.Subscribe<OnDragAttackJoystickEvent>(OnDragAttackJoystickHandler);
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

       
    } 

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(WaitForSecondsDuringFire(0.7f));
        }
           

        if (Vector3.Magnitude(MoveJoystick.Direction)>0)
        {
            _isMoving = true;

            if(!isFire){
                _animator.SetFloat("Speed", 100f);
                Rotate(transform,_angularSpeed, MoveJoystick);
            }
            
            

        }
        
        else if(MoveJoystick.Direction == Vector3.zero && !isFire) 
        {
            _isMoving = false;
            _animator.SetFloat("Speed", 0);

        }


        Rotate(_shootSector.transform, _angularSpeed, ShootJoystick);

    }

    private void FixedUpdate()
    {
        if (!isFire)
        {
            MoveUtility.NoFireMoveMod(_rigidBody, _isMoving, _speedNoFireMoveMod);
        }

        else if (isFire) {

            MoveUtility.FireMoveMod(_rigidBody,_animator, MoveJoystick, _isMoving, _speedFireMoveMod);
            
        }


    }

        private void OnDragAttackJoystickHandler(object sender, OnDragAttackJoystickEvent onDragAttackJoystickEvent)
        {
            Debug.Log("Hello");
        }

    private void Rotate(Transform transform, float angularSpeed, IJoystiсk joystickData)
    {
       transform.forward = Vector3.Lerp(transform.forward,new Vector3(joystickData.Direction.x, joystickData.Direction.z, joystickData.Direction.y), angularSpeed * Time.deltaTime);
    }
    

    IEnumerator WaitForSecondsDuringFire(float seconds)
    {
        _animator.SetBool("isFire", isFire = !isFire);

        yield return new WaitForSeconds(seconds);
        _animator.SetBool("isFire", isFire = !isFire);
    }



}

