using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MoveJoystick _moveJoystick;
    [SerializeField] private AttackJoystick _shootJoystick;

    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Player _playerAdamPrefab;
    [SerializeField] private Player _playerEvaPrefab;

    [SerializeField] private Bot _botAdamPrefab;

    [SerializeField] private Transform _startPlayerPosition;
    [SerializeField] private Transform _startBotPosition;

    void Start()
    {
        Player currentPlayer = CreatePlayer(BuildEvaPlayer(_playerEvaPrefab));
        BuildBotAdam(_botAdamPrefab, _startBotPosition, currentPlayer.transform);
        
        
    }

    Player CreatePlayer(Player PlayerCharacter)
    { 
        _camera.FollowedObject = PlayerCharacter.transform;
        PlayerCharacter.MoveJoystick = _moveJoystick;
        PlayerCharacter.ShootJoystick = _shootJoystick;
        return PlayerCharacter;
    }

    
    Player BuildAdamPlayer(Player prefab)
    {
        Player Adam = Instantiate(prefab, _startPlayerPosition.position, _startPlayerPosition.rotation);
        Adam.Weapon = Adam.GetComponentInChildren<MultiShotBlaster>();
        return Adam;
    }

    Player BuildEvaPlayer(Player prefab)
    {
        Player Eva = Instantiate(prefab, _startPlayerPosition.position, _startPlayerPosition.rotation);
        Eva.Weapon = Eva.GetComponentInChildren<OneShotBlaster>();
        return Eva;
    }

    Bot BuildBotAdam(Bot prefab, Transform initPos, Transform player)
    {
        Bot Adam = Instantiate(prefab, initPos.position, initPos.rotation);
        Adam.Weapon = Adam.GetComponentInChildren<MultiShotBlaster>();
        Adam.SetPlayer(player.transform);
        return Adam;
    }
}
