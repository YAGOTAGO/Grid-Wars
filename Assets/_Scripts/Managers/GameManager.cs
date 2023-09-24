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
    public GameState State;
    public bool IsWinner = true; //true by default loser swaps scene and sets this to false
    [SerializeField] private SceneAsset _endScene;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {

        State = newState;
        switch(newState)
        {

        }

    }





    [ServerRpc(RequireOwnership = false)]
    public void LoadEndSceneServerRPC()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(_endScene.name, LoadSceneMode.Single);
    }

}

public enum GameState 
{
    Starting = 0,
    LoadGrid = 1,
    LoadSurfaces=2,
    LoadCharacters = 3,
    EndGame =4,
}

