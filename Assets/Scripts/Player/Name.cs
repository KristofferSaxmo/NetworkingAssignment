using Unity.Collections;
using Unity.Netcode;

public class Name : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> playerName = new();
    
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        string userName = SavedClientInformationManager.GetUserData(NetworkObject.OwnerClientId).userName;
        playerName.Value = userName;
    }
}