using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
    public static PlayerSpawner Instance;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] public SceneAsset GameScene; //the scene which we want the characters to spawn in
    [SerializeField] private List<Vector3Int> _spawnLocations = new();

    private void Awake()
    {
        if(Instance == null) 
        { 
            Instance = this;
            DontDestroyOnLoad(this);
        } 
        else
        {
            Destroy(gameObject);
        }
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnCharacters;
    }

    private void SpawnCharacters(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        int positionIndex = 0;
        
        if(sceneName == GameScene.name)
        {
            if (IsServer)
            {
                foreach(ulong clientId in clientsCompleted)
                {
                    GameObject characterGO1 = Instantiate(_playerPrefab);
                    GameObject characterGO2 = Instantiate(_playerPrefab);
                    GameObject characterGO3 = Instantiate(_playerPrefab);

                    Character character1 = characterGO1.GetComponent<Character>();
                    Character character2 = characterGO2.GetComponent<Character>();
                    Character character3 = characterGO3.GetComponent<Character>();

                    characterGO1.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                    characterGO2.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                    characterGO3.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

                    character1.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                    character2.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                    character3.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                    
                }

            }

        }

    }



}
