using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    

    [SerializeField] private float _angularSpeed = 20f;
    [SerializeField] private float _speed = 3f;
    //[SerializeField] private Animator _rigAnimator;
    [SerializeField] private Button _shootButton;
    //[SerializeField] private Rig _rig; 

    private Rigidbody _rigidBody;
    private Animator _animator;

    bool isFire = false;

    public Joystick Move { get; set; }

    private bool _isMoving = false;
    float t;
    void Awake()
    {
        AnimationRigEventHandler.AnimationEvent += OnFireEventHandler;

        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _shootButton.onClick.AddListener(() => {

            StartCoroutine(WaitForSecondsDuringFire(2));
         
        });
       
    }

    
    void Update()
    {

        //_rig.weight = Mathf.Lerp(0, 1, t);
        //t += 0.7f * Time.deltaTime;
        //Debug.Log(_rig.weight);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(WaitForSecondsDuringFire(2));


        }
           // _rigAnimator.SetBool("isFire", isFire = !isFire);

        if (Vector3.Magnitude(Move._direction)>0 && !isFire)
        {
            _isMoving = true;

           // _animator.SetTrigger("BlendTreeTriger");


            _animator.SetFloat("Speed", 100f);
            Rotate();

        }
        
        else if(Move._direction == Vector3.zero && !isFire) 
        {
            _isMoving = false;
            _animator.SetFloat("Speed", 0);
           

        }


    }

    private void FixedUpdate()
    {
        if (!isFire)
        {
            if (_isMoving)
            {
                Debug.Log("First move mod is ON");
                _rigidBody.velocity = transform.forward * _speed;
            }
            else
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
            }
        }




        if (isFire) { 

            if (_isMoving) {
                Debug.Log("Second move mod is ON");
                _rigidBody.velocity = 2f*new Vector3(Move._direction.x, Move._direction.z, Move._direction.y);
                _animator.SetFloat("VelocityX", Move._direction.x);
                _animator.SetFloat("VelocityZ", Move._direction.y);

            }
            else{
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;
             }
        }



    }

        private void OnFireEventHandler(){
            _animator.SetBool("isFire", isFire = !isFire);
        }

    private void Rotate()
    {
       transform.forward = Vector3.Lerp(transform.forward,new Vector3(Move._direction.x, Move._direction.z, Move._direction.y), _angularSpeed * Time.deltaTime);
    }

   IEnumerator WaitForSecondsDuringFire(float seconds)
    {
        _animator.SetBool("isFire", isFire = !isFire);

        yield return new WaitForSeconds(seconds);
        _animator.SetBool("isFire", isFire = !isFire);
    }
}

