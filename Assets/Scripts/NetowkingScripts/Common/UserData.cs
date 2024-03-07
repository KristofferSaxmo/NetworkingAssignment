using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

public class UserData
{
    public string userName;
    public string authId;
    public ulong networkID;
}

public static class UserDataWrapper
{
    public static byte[] PayLoadInBytes()
    {
        UserData userData = new UserData
        {
            userName = PlayerPrefs.GetString("userName", "John doe"),
            authId = AuthenticationService.Instance.PlayerId
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);
        return payloadBytes;
    }
}