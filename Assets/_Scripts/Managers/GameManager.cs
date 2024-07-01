using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : NetworkSingleton<GameManager>
{
    [SerializeField] private GameObject _loadScreenObject;
    public int Round = 0;
    public bool IsWinner = true; //true by default loser swaps scene and sets this to false

    private void Start()=> _loadScreenObject.SetActive(true); //load screen up

    public void StartGame(MapsBase map)
    {
        WaitBoardLoadClientRpc(map.GetNumTiles()); //Client RPC cause both need to initialize neighboors
        GridManager.Instance.SpawnBoard(map);
    }

    [ClientRpc]
    private void WaitBoardLoadClientRpc(int tileNum)
    {
        StartCoroutine(WaitForBoardLoad(tileNum));
    }

    private IEnumerator WaitForBoardLoad(int numTiles)
    {
        yield return new WaitUntil(() => GridManager.Instance.GridCoordTiles.Count >= numTiles && GridManager.Instance.CubeCoordTiles.Count >= numTiles);

        //Cache neighboors
        GridManager.Instance.CacheNeighbors();

        //Spawn the characters;
        if (IsServer)
        {
            Debug.Log("SPAWN");
        }

        //Put load screen away
        _loadScreenObject.SetActive(false);
    }

}


