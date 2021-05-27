using GameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const int PlayerLayer = 8;
    public const int BotLayer = 9;
    [SerializeField] private int _scoreToWin = 15;

    [SerializeField] private LayerMask _botTeamAttackMask;
    [SerializeField] private LayerMask _playerTeamAttackMask;

    [SerializeField] private MoveJoystick _moveJoystick;
    [SerializeField] private AttackJoystick _shootJoystick;

    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Player _playerAdamPrefab;
    [SerializeField] private Player _playerEvaPrefab;

    [SerializeField] private Bot _botAdamPrefab;
    [SerializeField] private Bot _botEvaPrefab;

    [SerializeField] private Transform _startPlayerPosition;
    [SerializeField] private Transform _startBotAdamPosition;
    [SerializeField] private Transform _startBotEvaPosition;

    [SerializeField] private Transform _startAllyPosition;

    [SerializeField] private Text _playerTeamScore;
    [SerializeField] private Text _botTeamScore;

    private static int PlayerTeamScore = 0;
    private static int BotTeamScore = 0;


    Bot currentBotAdam;
    Bot currentBotEva;
    Bot currentAlly;
    Player currentPlayer;


    void Awake()
    {
        EventAggregator.Subscribe<PlayerKilledEvent>(OnPlayerKilledEventHandler);
        EventAggregator.Subscribe<BotKilledEvent>(OnBotKilledEventHandler);
        EventAggregator.Subscribe<CrystallAddedEvent>(OnCrystallAddedEventHandler);

        currentPlayer = CreatePlayer(BuildEvaPlayer(_playerEvaPrefab));

        currentBotAdam = BuildBotAdam(_botAdamPrefab, _startBotAdamPosition, currentPlayer.transform, BotLayer, _botTeamAttackMask);
        
        currentBotEva = BuildBotEva(_botEvaPrefab, _startBotEvaPosition, currentPlayer.transform, BotLayer, _botTeamAttackMask);
        
        currentAlly = BuildBotAdam(_botAdamPrefab, _startAllyPosition, currentBotAdam.transform, PlayerLayer, _playerTeamAttackMask);
        
    }

    Player CreatePlayer(Player PlayerCharacter)
    { 
        _camera.FollowedObject = PlayerCharacter.transform;
        PlayerCharacter.GetComponentInChildren<BillBoard>().cam = _camera.transform;
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


    public void RespawnPlayer()
    {
        currentPlayer = CreatePlayer(BuildEvaPlayer(_playerEvaPrefab));
        currentBotAdam.SetPlayer(currentPlayer.transform);
        currentBotEva.SetPlayer(currentPlayer.transform);
    }

    public void DeleteCamaraReference()
    {
        _camera.FollowedObject = null;
    }

    private void OnPlayerKilledEventHandler(object sender, PlayerKilledEvent playerKilledEvent)
    {
        DeleteCamaraReference();
        RespawnPlayer();
    }

    public void Restart()
    {
        DeleteCamaraReference();
        Destroy(currentBotAdam.gameObject);
        Destroy(currentBotEva.gameObject);
        Destroy(currentAlly.gameObject);
        Destroy(currentPlayer.gameObject);
    }

    #region EventHandlers
    private void OnCrystallAddedEventHandler(object sender, CrystallAddedEvent crystalAddedEvent)
    {
       if((sender as Character).gameObject.layer == PlayerLayer)
        {
            PlayerTeamScore += crystalAddedEvent.CrystalAmount;
            _playerTeamScore.text = PlayerTeamScore.ToString();

            if(PlayerTeamScore == _scoreToWin)
                {
                   // Restart();
                }
        }
        else if ((sender as Character).gameObject.layer == BotLayer)
        {
            Debug.Log("Bot");
            BotTeamScore += crystalAddedEvent.CrystalAmount;
            _botTeamScore.text = BotTeamScore.ToString();

            if (PlayerTeamScore == _scoreToWin)
                {             
                  //  Restart();
                }
        }
    }

    private void OnBotKilledEventHandler(object sender, BotKilledEvent botKilledEvent)
    {

        if (botKilledEvent.CharacterType == CharacterType.Adam && botKilledEvent.Layer % 32 != PlayerLayer)
        {
            currentBotAdam = BuildBotAdam(_botAdamPrefab, _startBotAdamPosition, currentPlayer.transform, PlayerLayer, botKilledEvent.AttackLayer);
            //currentBotAdam.GetComponent<MeshRenderer>().material.color = Color.red;
        }
            

        else if(botKilledEvent.CharacterType == CharacterType.Adam && botKilledEvent.Layer % 32 == PlayerLayer)
        {
            currentAlly = BuildBotAdam(_botAdamPrefab, _startAllyPosition, currentPlayer.transform, BotLayer, botKilledEvent.AttackLayer);
            //currentAlly.GetComponent<MeshRenderer>().material.color = Color.white;
        }
            


        if (botKilledEvent.CharacterType == CharacterType.Eva && botKilledEvent.Layer % 32 != PlayerLayer)
        {
            currentBotEva = BuildBotEva(_botEvaPrefab, _startBotEvaPosition, currentPlayer.transform, PlayerLayer, botKilledEvent.AttackLayer);
            //currentBotEva.GetComponent<MeshRenderer>().material.color = Color.red;
        }
            

        else if(botKilledEvent.CharacterType == CharacterType.Eva && botKilledEvent.Layer % 32 == PlayerLayer)
        {
            currentAlly = BuildBotEva(_botEvaPrefab, _startAllyPosition, currentPlayer.transform, BotLayer, botKilledEvent.AttackLayer);
            //currentAlly.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    #endregion
}

