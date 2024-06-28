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
    [SerializeField] private GameObject _loadScreenObject;
    [SerializeField] private string _endScene;
    public int Round = 0;
    public bool IsWinner = true; //true by default loser swaps scene and sets this to false

    private void Awake()
    {
        Instance = this;
    }

    private void Start()=> _loadScreenObject.SetActive(true); //load screen up

    public void StartGame(MapsBase map)
    {
        Debug.Log("Inside Start Game");
        GridManager.Instance.SpawnBoard(map);
    }
    

}

public enum GameState 
{
    StartState = 0,
    LoadGrid = 1,
    LoadCharacters = 3,
    EndLoadScreen = 4,
    EndGame = 5,
}

