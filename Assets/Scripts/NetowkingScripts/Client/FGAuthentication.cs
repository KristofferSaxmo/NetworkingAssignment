using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Rendering;

public static class FGAuthentication
{
public static AuthState currentAuth = AuthState.NotAuth;

public static async Task<AuthState> FGAuthen(int maxTries){

if(currentAuth == AuthState.Authorized) return currentAuth;

int tries = 0;

currentAuth = AuthState.Authorizing;

while(currentAuth == AuthState.Authorizing && tries < maxTries){

    await AuthenticationService.Instance.SignInAnonymouslyAsync();
    if(AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized){
        Debug.LogWarning("We have been Autherized");
        currentAuth = AuthState.Authorized;
        break;
    }
    tries++;

    await Task.Delay(1000);
}

return currentAuth;

}



public enum AuthState
{
    NotAuth,
    Authorizing,
    Authorized
}


}
