using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostCodeFetcher : NetworkBehaviour
{
    [SerializeField]  Text joinCodeText;

    public override void OnNetworkSpawn()
    {
        if(!IsHost) gameObject.SetActive(false);
        joinCodeText.text = HostSingelton.GetInstance().GetJoinCode();
    }
}
