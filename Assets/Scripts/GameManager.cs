using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerMovement _player;
    

    void Start()
    {
        CreatePlayer();
        
    }


    void Update()
    {
        

    }

    void CreatePlayer()
    {
        _player.Move = _joystick;
    }

   
    
}
