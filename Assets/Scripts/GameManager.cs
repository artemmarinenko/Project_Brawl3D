using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MoveJoystick _moveJoystick;
    [SerializeField] private AttackJoystick _shootJoystick;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Player _playerAdamPrefab;
    [SerializeField] private Transform _startPlayerPosition;

    void Start()
    {
        CreatePlayerAdam(BuildAdamPlayer(_playerAdamPrefab));
        
    }


    void CreatePlayerAdam(Player PlayerCharacter)
    { 
        _camera.FollowedObject = PlayerCharacter.transform;
        PlayerCharacter.MoveJoystick = _moveJoystick;
        PlayerCharacter.ShootJoystick = _shootJoystick;
    }
    Player BuildAdamPlayer(Player prefab)
    {
        Player Adam = Instantiate(prefab, _startPlayerPosition.position, _startPlayerPosition.rotation);
        Adam.Weapon = Adam.GetComponentInChildren<Blaster>();
        return Adam;
    }
   
    
}
