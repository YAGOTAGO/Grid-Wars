using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    public static CharacterSpawner Instance;

    public List<Character> ServerCharacters = new();
    public List<Character> ClientCharacters = new();

    private List<Vector3Int> _spawnPosServer = new();
    private List<Vector3Int> _spawnPosClient = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetSpawnPoints(MapsBase map)
    {
        _spawnPosServer = map.SpawnPosServer;
        _spawnPosClient = map.SpawnPosClient;
    }


}
