using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private string _sceneName; //the scene which we want the characters to spawn in

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

        if(sceneName == _sceneName)
        {
            if (IsServer)
            {
                Debug.Log("Server spawning characters " + OwnerClientId);

                GameObject character1 = Instantiate(_playerPrefab);
                GameObject character2 = Instantiate(_playerPrefab);
                GameObject character3 = Instantiate(_playerPrefab);

                character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(-6, -17)], true);
                character2.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(-6, -5)], true);
                character3.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(-6, 7)], true);

                character1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
                character2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
                character3.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);

            }
            else
            {
                SpawnClientCharactersServerRPC(OwnerClientId);
            }

        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnClientCharactersServerRPC(ulong clientID)
    {
        Debug.Log("Client spawning characters " + OwnerClientId);
        GameObject character1 = Instantiate(_playerPrefab);
        GameObject character2 = Instantiate(_playerPrefab);
        GameObject character3 = Instantiate(_playerPrefab);

        character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(12, -17)], true);
        character2.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(13, -4)], true);
        character3.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(12, 9)], true);

        character1.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
        character2.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
        character3.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
    }


}
