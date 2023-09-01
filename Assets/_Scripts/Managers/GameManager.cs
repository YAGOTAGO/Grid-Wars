using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _characterPrefabToSpawn;
    private GameObject _prefabInstance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

    }

    private void Start()
    {
        
        
        //GameObject character1 = Instantiate(_characterPrefabToSpawn);
        //character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)], true);
        
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.T)) 
        {
            SpawnCharacters();
        }
    }
    private void SpawnCharacters()
    {
        Debug.Log("In spawn characters");

        if (_characterPrefabToSpawn == null) { Debug.Log("no prefab to spawn"); return; }

        _prefabInstance = Instantiate(_characterPrefabToSpawn);

        if (_prefabInstance == null) { Debug.Log("Prefab instance is null"); }

        _prefabInstance.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)], true);
        NetworkObject spawnedNetworkObject = _prefabInstance.GetComponent<NetworkObject>();
        spawnedNetworkObject.Spawn();

        Debug.Log("Called character spawn");
    }
    

}
