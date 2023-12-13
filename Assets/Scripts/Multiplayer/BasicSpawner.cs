using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
   
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    private NetworkRunner _runner;

    
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

    // private void Awake()
    // {
    //     StartGame(GameMode.Host);
    // }

    // private void Start()
    // {
    //     if(PlayerPrefs.GetInt("isHost") == 1) StartGame(GameMode.Host);
    //     else StartGame(GameMode.Client);
    //     Debug.Log("STARTTTTTT");
    // }
    
    // private void OnGUI()
    // {
    //     if (_runner == null)
    //     {
    //         if (GUI.Button(new Rect(0,0,200,40), "Host"))
    //         {
    //             StartGame(GameMode.Host);
    //         }
    //         if (GUI.Button(new Rect(0,40,200,40), "Join"))
    //         {
    //             StartGame(GameMode.Client);
    //         }
    //     }
    // }

    public void JoinGame(bool host)
    {
        if (host)
        {
            Debug.Log("HOST");
            StartGame(GameMode.Host);
        }
        else
        {
            Debug.Log("CLIENT");
            StartGame(GameMode.Client);
        }

         
    }

    async void StartGame(GameMode mode)
    {
       
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "Room",
            Scene = 3, // scene index 3
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        
    }
    
    

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
      
        Debug.Log("MODE --- "+runner.IsServer);
        if (runner.IsServer)
        {
        
          //  Debug.Log("PLAYER JOIN "+player.PlayerId );
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3(5,105,20);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            _spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    
    public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveVertical > 0)
            {
                data.direction += Vector3.forward;
              //  Debug.Log("UP");
            }
        
            if (moveVertical < 0)
            {
                data.direction += Vector3.back;
               // Debug.Log("DOWN");
            }
        

            if (moveHorizontal < 0)
            {
                data.direction += Vector3.left;
               // Debug.Log("LEFT");
            }
         

            if (moveHorizontal > 0)
            {
                data.direction += Vector3.right;
              //  Debug.Log("RIGHT");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                data.direction += Vector3.up;
            }
               
    
            input.Set(data);
        }
}
