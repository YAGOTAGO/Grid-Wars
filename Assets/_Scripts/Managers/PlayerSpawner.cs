using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private string _sceneName; //the scene which we want the characters to spawn in
    [SerializeField] private List<Vector3Int> _spawnLocations = new();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnCharacters;
    }

    private void SpawnCharacters(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        int positionIndex = 0;
        
        if(sceneName == _sceneName)
        {
            if (IsServer)
            {
                foreach(ulong clientId in clientsCompleted)
                {
                    GameObject character1 = Instantiate(_playerPrefab);
                    GameObject character2 = Instantiate(_playerPrefab);
                    GameObject character3 = Instantiate(_playerPrefab);

                    character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                    character2.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                    character3.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);

                    character1.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                    character2.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                    character3.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                }

            }

        }

    }



}
