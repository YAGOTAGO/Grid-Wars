using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public bool IsWinner = true; //true by default loser swaps scene and sets this to false
    [SerializeField] private SceneAsset _endScene;
    
    private void Awake()
    {
        Instance = this;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void LoadEndSceneServerRPC()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_endScene.name, LoadSceneMode.Single);
    }
    
}
