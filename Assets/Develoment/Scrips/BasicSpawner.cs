using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    #region Parameters
    [SerializeField] GameObject Button;
    private NetworkRunner _runner;
    [SerializeField] GameObject Panel;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    Player[] players;
    public int IdPlayer;
    public int MaxPlayersRoom;
    [SerializeField] Text ServerName;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    #endregion
    #region NetworkFuctions
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    { // Create a unique position for the player
        Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

        // Keep track of the player avatars so we can remove it when they disconnect
        _spawnedCharacters.Add(player, networkPlayerObject);
        IdPlayer = player.PlayerId;
        if(IdPlayer == MaxPlayersRoom-1) Button.SetActive(true);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {  // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
        var data = new NetworkInputData();
        data.Force = Input.GetAxis("Vertical");
        data.turn = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.Space)) data.Break = true;
        input.Set(data);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public async void StartGame(GameMode mode, string Name)
    {      
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
      
        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = Name,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = MaxPlayersRoom //maximo de players,
        });
    }
    #endregion
    #region GameFuctions
    public void Buttons(string Mode)
    {
        string NameServer = ServerName.text;
        if (NameServer == "") NameServer = "No name";
        if (_runner == null)
        {
            if(Mode == "Host")
            {
               StartGame(GameMode.Host,NameServer);
            }
            if (Mode == "AutoHostOrClient")
            {
               StartGame(GameMode.AutoHostOrClient,NameServer) ;
            }
            Panel.SetActive(false);
        }
    }
    
    public void InitRace()
    {      
        players = FindObjectsOfType<Player>();
        FindObjectOfType<Player>().RPC_InitGame(players);
        Button.SetActive(false);
        FindObjectOfType<GameManager>().NoGameInit = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }
    #endregion
} 