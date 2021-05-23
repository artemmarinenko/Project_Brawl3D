using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MoveJoystick _moveJoystick;
    [SerializeField] private AttackJoystick _shootJoystick;
    [SerializeField] private Player _player;
    

    void Start()
    {
        CreatePlayer();
        
    }


    void Update()
    {
        

    }

    void CreatePlayer()
    {
        _player.MoveJoystick = _moveJoystick;
        _player.ShootJoystick = _shootJoystick;
    }

   
    
}
