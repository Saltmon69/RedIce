using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;

public class CloudSaveManager : MonoBehaviour
{
    public List<GameObject> objectsToSave = new List<GameObject>();
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        SaveData();
    }

    public async void SaveData()
    {
        var playerData = new Dictionary<string, object>{
            {"firstKeyName", "a text value"},
            {"Player", objectsToSave[1]}
        };
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }
}
