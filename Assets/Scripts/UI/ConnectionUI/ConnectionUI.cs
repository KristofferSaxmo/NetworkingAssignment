using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class ConnectionUI : MonoBehaviour
{

    private UIDocument _uIDocument;
    private VisualElement _rootElement;
    private Button _clientJoinBUtton;
    private Button _serverConnectButton;

    private Label _connectionLable;

    // Start is called before the first frame update
    void Start()
    {
        _uIDocument = GetComponent<UIDocument>();
        if (_uIDocument == null) return;
        _rootElement = _uIDocument.rootVisualElement;

        _clientJoinBUtton = _rootElement.Q<Button>("ClientConnectButton");
        _clientJoinBUtton.clicked += JoinServer;


        _serverConnectButton = _rootElement.Q<Button>("ServerConnectButton");
        _serverConnectButton.clicked += HostAGame;

        _connectionLable = _rootElement.Q<Label>("ConnectionLable");
    }


    private void HostAGame()
    {
        NetworkManager.Singleton.OnServerStarted += HostStarted;
        NetworkManager.Singleton.StartHost();
    }


    private void JoinServer()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        NetworkManager.Singleton.StartClient();
        HideConnectionButtons();
        _connectionLable.text = "Looking for the Host to connect";

    }

    private void HideConnectionButtons()
    {
        _clientJoinBUtton.AddToClassList("notVisible");
        _serverConnectButton.AddToClassList("notVisible");
    }
    private void HostStarted()
    {
        _connectionLable.text = "Hosting Game with an id: " + NetworkManager.Singleton.LocalClientId ; 
         HideConnectionButtons();
    }

    private void ClientConnected(ulong clientID)
    {
        _connectionLable.text = "I am a client with an id : " + clientID;
        HideConnectionButtons();
    }



}
