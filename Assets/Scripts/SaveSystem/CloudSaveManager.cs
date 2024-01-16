using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using VInspector;

public class CloudSaveManager : MonoBehaviour
{
    public List<GameObject> objectsToSave = new List<GameObject>();
    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        SaveData();
    }

    [Button("Save Data")]
    public async void SaveData()
    {
        var playerData = new Dictionary<string, object>{
            {"Player", objectsToSave[0]},
            {"PlayerPosition", objectsToSave[0].transform.position},
            {"PlayerRotation", objectsToSave[0].transform.rotation},
            {"PlayerScale", objectsToSave[0].transform.localScale}
        };
        var result = await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
        Debug.Log($"Saved data {string.Join(',', playerData)}");
    }
}
