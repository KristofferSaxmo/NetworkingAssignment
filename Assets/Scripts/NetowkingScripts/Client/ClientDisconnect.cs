using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ClientDisconnect 
{
   NetworkManager networkManager;
    public ClientDisconnect (NetworkManager networkManager){
        this.networkManager = networkManager;
        networkManager.OnClientStarted += onClientStartedAlready;
    }

    private void onClientStartedAlready()
    {
        networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientID)
    {
        if(networkManager.IsClient && networkManager.IsConnectedClient && networkManager.LocalClientId == clientID && clientID !=0 ){
            networkManager.Shutdown();
            SceneManager.LoadScene("MainMenuScene");
        }

    }

  
}
