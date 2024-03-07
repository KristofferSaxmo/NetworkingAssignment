using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HostSingelton : Singleton<HostSingelton>
{
  HostManager hostManager;


  public async Task<bool> InitSeverAsync()
  {
    await Task.Delay(0);
    hostManager = ScriptableObject.CreateInstance<HostManager>();
    return true;
  }

  public async Task StartHost()
  {
    await hostManager.StartHostAsync();
    InvokeRepeating(nameof(PingServer), 10, 10);
  }

  private void PingServer()
  {
    hostManager.PingServer();
  }

  public String GetJoinCode()
  {
    return hostManager.joinCode;
  }

  public String GetLobbyID()
  {
    return hostManager.lobbyID;
  }
}
