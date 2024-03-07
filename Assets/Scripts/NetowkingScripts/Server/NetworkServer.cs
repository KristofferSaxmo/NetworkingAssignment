using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer
{

    NetworkManager networkManager;


    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;
        networkManager.ConnectionApprovalCallback += ConnectionApproval;
        NetworkManager.Singleton.OnClientDisconnectCallback += clientDisconnect;
    }

    private void clientDisconnect(ulong networkID)
    {
       SavedClientInformationManager.RemoveClient(networkID);
    }

    private void ConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);
        userData.networkID = request.ClientNetworkId;
        
        response.Approved = true;

        SavedClientInformationManager.AddClient(userData);

        response.CreatePlayerObject = true; // Theo

    }
}
