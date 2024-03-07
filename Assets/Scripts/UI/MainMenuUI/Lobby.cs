using System;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;


public class Lobby : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _rootElement;

    private Button _refreshButton;
    private Button _backButton;

    [SerializeField] GameObject mainMenuGO;

    private VisualElement _listOfLobbies;
    private ScrollView _listLobbiesScrollView;


    bool isRefreshing = false;

    private void Awake()
    {
        _uIDocument = GetComponent<UIDocument>();
    }


    private void OnEnable()
    {
        if (_uIDocument == null) return;
        _rootElement = _uIDocument.rootVisualElement;
        _listOfLobbies = _rootElement.Q<VisualElement>("ListOfLobbies");
        _listLobbiesScrollView = _listOfLobbies?.Q<ScrollView>();
        _backButton = _rootElement.Q<Button>("BackButton");
        _refreshButton = _rootElement.Q<Button>("RefreshButton");
        _backButton.clicked += BackToMainMenu;
        _refreshButton.clicked += RefreshLobbies;
        //_listLobbiesScrollView.Add(CreateLobbyContainer(i));
        RefreshLobbies();




    }

    private void OnDisable()
    {
        _backButton.clicked -= BackToMainMenu;
        _refreshButton.clicked -= RefreshLobbies;
    }



 
 private VisualElement CreateLobbyContainer(String userName, String joinCode, int currentPlayerCount, int maxPlayerCount)
    {
        VisualElement lobbyContainer = new VisualElement();
        lobbyContainer.AddToClassList("lobbyContainer");
        
        // Add a Label to the LobbyContainer for Lobby Name
        Label lobbyNameLabel = new Label(userName);
        lobbyNameLabel.AddToClassList("leftLabelLobby");
        lobbyContainer.Add(lobbyNameLabel);

        // Add a Button to the LobbyContainer for joining the lobby
        Button joinButton = new Button();
        joinButton.text = "Join Lobby";
        joinButton.AddToClassList("defaultButtonCLass");

        joinButton.clicked += async ()=>{
             await  ClientSingelton.GetInstance().StartClientAsync(joinCode);
        };

        lobbyContainer.Add(joinButton);

        // Add a Label to the LobbyContainer for Lobby Status
        Label lobbyStatusLabel = new Label(currentPlayerCount+"/"+maxPlayerCount);
        lobbyStatusLabel.AddToClassList("rightLabelLobby");
        lobbyContainer.Add(lobbyStatusLabel);

        return lobbyContainer;
    }

    private async void RefreshLobbies()
    {
        if (isRefreshing) return;
        isRefreshing = true;
        QueryLobbiesOptions options = new QueryLobbiesOptions();
        options.Count = 30;

        options.Filters = new List<QueryFilter>(){
                new QueryFilter (
                    field:QueryFilter.FieldOptions.AvailableSlots,
                    op:QueryFilter.OpOptions.GT,
                    value:"0"),
                    new QueryFilter (
                    field:QueryFilter.FieldOptions.IsLocked,
                    op:QueryFilter.OpOptions.EQ,
                    value:"0")
        };

        QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

        _listLobbiesScrollView.Clear();
        foreach (var lobby in lobbies.Results)
        {
            var joiningLobbyInstance = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobbyInstance.Data["JoinCode"].Value;
            int currentPlayerCount = lobby.Players.Count;
            int maxPlayerCount = lobby.MaxPlayers;
            
            VisualElement lobbyCountainer = CreateLobbyContainer("Uknown Lobby Name", joinCode, currentPlayerCount, maxPlayerCount);
            _listLobbiesScrollView.Add(lobbyCountainer);
        }


    }

    private void BackToMainMenu()
    {
        mainMenuGO.SetActive(true);
        gameObject.SetActive(false);
    }


   
}