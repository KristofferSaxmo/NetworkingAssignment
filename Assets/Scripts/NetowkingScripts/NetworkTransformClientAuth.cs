using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class NetworkTransformClientAuth : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
       // https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@1.8/api/Unity.Netcode.Components.NetworkTransform.html
        return false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }
    protected override void Update()
    {

        base.Update();


        if(!IsOwner) return;
        if(IsHost || IsServer) return;
        

        CanCommitToTransform = IsOwner;
        if(NetworkManager.IsConnectedClient){
            TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
        }

    }

}
