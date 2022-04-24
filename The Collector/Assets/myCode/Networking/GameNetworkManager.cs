using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Netcode.Transports.Facepunch;
using Steamworks.Data;
using Steamworks;
using System;
using Unity.Netcode;
//using UnityEngine.Networking;
public class GameNetworkManager : MonoBehaviour
{
    public static GameNetworkManager Instance { get; private set; } = null;
    public Lobby? currentLobby { get; private set; } = null;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad (gameObject);
    }
    private FacepunchTransport transport = null;
    private void Start()
    {
        transport = GetComponent<FacepunchTransport>();
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
    }
    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= OnLobbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;

        if (NetworkManager.Singleton == null) return;
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    }
    public void OnApplicationQuit()
    {
        Disconnect();
    }
    public async void StartHost(int maxMembers = 4)
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        
        NetworkManager.Singleton.StartHost();

        currentLobby = await SteamMatchmaking.CreateLobbyAsync(maxMembers);
    }
    public void StartClient(SteamId id)
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        transport.targetSteamId = id;
        if (NetworkManager.Singleton.StartClient()) Debug.Log("Client has joined",this);
    }
    public void Disconnect()
    {
        currentLobby?.Leave();
        if(NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.Shutdown();
    }
    #region Unity Network Callbacks
    private void OnServerStarted()
    {
        Debug.Log("Server has started",this);
    }
    private void OnClientConnectedCallback(ulong clientID)
    {
        Debug.Log($"Client connected, ClientID = {clientID}");
    }
    private void OnClientDisconnectCallback(ulong clientID)
    {
        Debug.Log($"Client disconnected, ClientID = {clientID}");
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
    } 
    #endregion
    #region Steamworks Callbacks
    private void OnLobbyCreated(Result result, Lobby lobby)
    {
       if(result != Result.OK)
        {
            Debug.LogError($"Lobby couldn't be created, {result}", this);
            return;
        }
        lobby.SetFriendsOnly();
        lobby.SetData("name", "Cool Lobby");
        lobby.SetJoinable(true);
        Debug.Log("Lobby has been created");
    }
    private void OnLobbyEntered(Lobby lobby)
    {
        if (NetworkManager.Singleton.IsHost) return;
        StartClient(lobby.Id);//if doesnt work use lobby.Owner.Id as parameter instead
    }
    private void OnLobbyInvite(Friend friend, Lobby lobby)
    {
        Debug.Log($"You got an invite from {friend.Name}", this);
    }
    private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        StartClient(id); //might not be needed as on lobby entered already starts client. If not use code below
        //currentLobby = lobby;
        //currentLobby?.Join();
    }
    private void OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        throw new NotImplementedException();
    }

    private void OnLobbyMemberLeave(Lobby lobby, Friend friend)
    {
        throw new NotImplementedException();
    }

    private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
        throw new NotImplementedException();
    } 
    #endregion
}
