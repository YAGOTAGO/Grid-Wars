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
    public GameState State= GameState.StartState;
    [SerializeField] private GameObject _loadScreenObject;
    [SerializeField] private string _endScene;
    public int Round = 0;
    public bool IsWinner = true; //true by default loser swaps scene and sets this to false

    private void Awake()
    {
        Instance = this;
    }

    private void Start()=> ChangeState(GameState.LoadGrid);

    public void ChangeState(GameState state)
    {
        State = state;
        switch(state)
        {
            case GameState.LoadGrid: LoadGrid(); break;
            case GameState.InitNeighboors: InitHexNeighboorsClientRPC(); break;
            case GameState.LoadCharacters: SpawnCharacters(); break;
            case GameState.EndLoadScreen: EndLoadScreen(); break;
            case GameState.EndGame: EndGame(); break;
        }

    }

    private void EndGame()
    {
        if(IsServer)
        {
            EndGameClientRPC();
        }
        else
        {
            EndGameServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void EndGameServerRPC()
    {
        EndGameClientRPC();
    }

    [ClientRpc]
    private void EndGameClientRPC()
    {
        //Find out if won
        if(Database.Instance.AllyCharacters.Count == 0)
        {
            IsWinner = false;
        }
        else
        {
            IsWinner= true;
        }

        if(IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }

        //After leaving network we go to end game scene
        SceneManager.LoadScene(_endScene);
        
        //Cleanup the network manager
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }

    private void EndLoadScreen()
    {
        _loadScreenObject.SetActive(false);
        EndLoadScreenServerRPC();
        StartCoroutine(StartGameDraw());
    }

    private bool AreCharactersLoaded()
    {
        return Database.Instance.AbstractCharactersDB.Count == 6;
    }
    private IEnumerator StartGameDraw()
    {
        //wait until all characters are spawned
        yield return new WaitUntil(AreCharactersLoaded);
        DeckManager.Instance.DeckDraw(2);
        DrawInitialHandServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DrawInitialHandServerRPC()
    {
        DeckManager.Instance.DeckDraw(2);
    }

    [ServerRpc(RequireOwnership = false)]
    private void EndLoadScreenServerRPC()
    {
        _loadScreenObject.SetActive(false);
    }

    private void SpawnCharacters()
    {
        SpawnCharactersServerRPC();
        ChangeState(GameState.EndLoadScreen);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCharactersServerRPC() 
    {
        PlayerSpawner.Instance.SpawnCharacters();
    }
    private void LoadGrid()
    {
        _loadScreenObject.SetActive(true); //load screen up
        if(IsServer) 
        {
            GridManager.Instance.InitBoard();
            ChangeState(GameState.InitNeighboors);
        }
    }

    [ClientRpc]
    private void InitHexNeighboorsClientRPC()
    {
        StartCoroutine(GridManager.Instance.InitNeighboors());
    }

}

public enum GameState 
{
    StartState = 0,
    LoadGrid = 1,
    InitNeighboors = 2,
    LoadCharacters = 3,
    EndLoadScreen = 4,
    EndGame = 5,
}

