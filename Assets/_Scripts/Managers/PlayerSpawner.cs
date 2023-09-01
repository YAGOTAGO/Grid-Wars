using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Scene _sceneToSpawnCharactersIn; //the scene which we want the characters to spawn in

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnCharacters;
    }

    private void SpawnCharacters(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        Debug.Log("Spawn characters called " + OwnerClientId);
        if(sceneName == _sceneToSpawnCharactersIn.name)
        {
            foreach(ulong id in clientsCompleted)
            {
                

            }

        }

    }

}
