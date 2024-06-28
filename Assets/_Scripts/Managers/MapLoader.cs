using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapLoader : NetworkBehaviour
{
    public static MapLoader Instance { get; private set; }

    [SerializeField] private List<MapsBase> _maps = new();
    [SerializeField] private TMP_Dropdown _mapDropdown;

    private Dictionary<string, MapsBase> _stringToMapBase = new();
    private string _currMapSelection = "Random";
    private int _numTiles = 0;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SetDropdown();
    }

    private void SetDropdown()
    {
        List<string> options = new() { "Random" };
        foreach(MapsBase map in _maps)
        {
            options.Add(map.MapName);
            _stringToMapBase.Add(map.MapName, map);
        }

        _mapDropdown.ClearOptions();
        _mapDropdown.AddOptions(options);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
        }
    }
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {

        //Server is responsible for calling the start of the game
        if (IsServer && sceneName == "GameScene")
        {
            MapsBase map = _currMapSelection == "Random" ? _stringToMapBase.Values.ElementAt(UnityEngine.Random.Range(0, _stringToMapBase.Count)) : _stringToMapBase[_currMapSelection];

            if (map == null) { Debug.LogError("MAP NOT FOUND IN MAP LOADER!"); }

            //Send an RPC to begin wait for hex neighboors
            Tilemap tilemap = map.TileMap;
            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    _numTiles++;
                }
            }
            WaitHexNeighborsClientRpc(_numTiles);

            //Send map and Character info to game manager
            Debug.Log("Scene Loaded Star Game send");
            GameManager.Instance.StartGame(map);

        }
    }

    [ClientRpc]
    void WaitHexNeighborsClientRpc(int tileNum)
    {
        StartCoroutine(GridManager.Instance.CacheNeighbors(tileNum));
    }

    private void OnEnable()
    {
        _mapDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDisable()
    {
        _mapDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int value)
    { 
        _currMapSelection = _mapDropdown.options[value].text.ToString();
    }

    /*public IEnumerator SpawnCharacters()
    {
       yield return new WaitUntil(() => GridManager.Instance.MapLoaded);
    }*/


}
