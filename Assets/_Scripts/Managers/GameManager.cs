using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

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
        
    }
    
}
