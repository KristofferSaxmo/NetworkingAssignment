using System;
using System.Threading.Tasks;
using UnityEngine;

public class ClientSingelton : Singleton<ClientSingelton>
{
    public ClientManager clientManager;
    public bool isAuth = false;

    public async Task InitClientAsync(){
        clientManager = ScriptableObject.CreateInstance<ClientManager>();
        isAuth = await clientManager.InitAsync();
    }

    public async Task StartClientAsync(String joinCode){
        await clientManager.StartClientAsync(joinCode);
    }
    

}
