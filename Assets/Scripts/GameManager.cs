using GameEvents;
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
    [SerializeField] private Bot _botEvaPrefab;

    [SerializeField] private Transform _startPlayerPosition;
    [SerializeField] private Transform _startBotPosition;

    Bot currentBot;
    Player currentPlayer;

    void Awake()
    {
        EventAggregator.Subscribe<PlayerKilledEvent>(OnPlayerKilledEventHandler);
        EventAggregator.Subscribe<BotKilledEvent>(OnBotKilledEventHandler);

        currentPlayer = CreatePlayer(BuildEvaPlayer(_playerEvaPrefab));
        currentBot = BuildBotAdam(_botAdamPrefab, _startBotPosition, currentPlayer.transform, 9, LayerMask.GetMask(new string[] { "PlayerTeam" }));
        
    }

    Player CreatePlayer(Player PlayerCharacter)
    { 
        _camera.FollowedObject = PlayerCharacter.transform;
        PlayerCharacter.GetComponentInChildren<BillBoard>().cam = _camera.transform;
        PlayerCharacter.MoveJoystick = _moveJoystick;
        PlayerCharacter.ShootJoystick = _shootJoystick;
        return PlayerCharacter;
    }

    public void Restart()
    {

        currentPlayer = CreatePlayer(BuildEvaPlayer(_playerEvaPrefab));
        currentBot.SetPlayer(currentPlayer.transform);
    }
    
    public void DeleteCamaraReference()
    {
        _camera.FollowedObject = null;
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

    Bot BuildBotAdam(Bot prefab, Transform initPos, Transform player, int Layer, LayerMask attackLayer)
    {
        Bot Adam = Instantiate(prefab, initPos.position, initPos.rotation);
        Adam.Weapon = Adam.GetComponentInChildren<MultiShotBlaster>();
        Adam.SetPlayer(player.transform);
        Adam.GetComponentInChildren<BillBoard>().cam = _camera.transform;
        Adam.gameObject.layer = Layer;
        Adam.SetLayerMask(attackLayer);
        return Adam;
    }

    Bot BuildBotEva(Bot prefab, Transform initPos, Transform player, int Layer, LayerMask attackLayer)
    {
        Bot Eva = Instantiate(prefab, initPos.position, initPos.rotation);
        Eva.Weapon = Eva.GetComponentInChildren<OneShotBlaster>();
        Eva.SetPlayer(player.transform);
        Eva.GetComponentInChildren<BillBoard>().cam = _camera.transform;
        Eva.gameObject.layer = Layer;
        Eva.SetLayerMask(attackLayer);
        return Eva;
    }


    private void OnPlayerKilledEventHandler(object sender, PlayerKilledEvent playerKilledEvent)
    {
        DeleteCamaraReference();       
        Restart();
    }


    private void OnBotKilledEventHandler(object sender, BotKilledEvent botKilledEvent)
    {
        Debug.Log(botKilledEvent.AttackLayer.value);

        if (botKilledEvent.CharacterType == CharacterType.Adam) 
            currentBot = BuildBotAdam(_botAdamPrefab, _startBotPosition, currentPlayer.transform, botKilledEvent.Layer, botKilledEvent.AttackLayer);
        if(botKilledEvent.CharacterType == CharacterType.Eva)
            currentBot = BuildBotAdam(_botEvaPrefab, _startBotPosition, currentPlayer.transform, botKilledEvent.Layer, botKilledEvent.AttackLayer);
    }
}
