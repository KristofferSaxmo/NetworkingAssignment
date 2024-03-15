using Unity.Collections;
using Unity.Netcode;

public class KillTracker : NetworkBehaviour
{
    public NetworkList<ulong> PlayerIDs { get; private set; } = new();
    public NetworkList<FixedString64Bytes> PlayerNames { get; private set; } = new();
    public NetworkList<int> PlayerKills { get; private set; } = new();

        
    public static KillTracker Instance { get; private set; }

    private void Awake()  
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddKill(ulong playerNetworkID)
    {
        if (!IsServer) return;
        
        if (!IsValidPlayer(playerNetworkID)) return;
        
        int index = PlayerIDs.IndexOf(playerNetworkID);
        if (index != -1)
            PlayerKills[index]++;
        else
        {
            PlayerIDs.Add(playerNetworkID);
            PlayerNames.Add(SavedClientInformationManager.GetUserData(playerNetworkID).userName);
            PlayerKills.Add(1);
        }
    }
    
    private bool IsValidPlayer(ulong playerNetworkID)
    {
        var clients = SavedClientInformationManager.GetAllClient();
        
        foreach (var client in clients)
            if (client.networkID == playerNetworkID)
                return true;
        
        return false;
    }
}