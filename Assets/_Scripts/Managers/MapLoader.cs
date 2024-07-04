using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapLoader : PersistentNetworkSingleton<MapLoader>
{
    public NetworkVariable<int> NumOfCharacters = new();

    [SerializeField] private List<MapsBase> _maps = new();
    [SerializeField] private TMP_Dropdown _mapDropdown;

    private readonly Dictionary<string, MapsBase> _stringToMapBase = new();
    private string _currMapSelection = "Random";
    private MapsBase _selectedMap;

    protected override void Awake()
    {
        base.Awake();
        SetDropdown();
    }
    
    //Just server has finished loading
    private void SceneManager_OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if(IsServer && sceneName == "SelectScene")
        {
            //Pick a map and have that information be known
            _selectedMap = _currMapSelection == "Random" ? _stringToMapBase.Values.ElementAt(UnityEngine.Random.Range(0, _stringToMapBase.Count)) : _stringToMapBase[_currMapSelection];
            if (_selectedMap == null) { Debug.LogError("MAP NOT FOUND IN MAP LOADER!"); }

            //Set the net var for number of people that can spawn
            NumOfCharacters.Value = _selectedMap.NumOfCharacters;

            //Set Spawn Points in character spawner
            CharacterSpawner.Instance.SetSpawnPoints(_selectedMap);

        }
    }

    //When both client and server have finished loading spawned objects
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        //Server is responsible for calling the start of the game
        if (IsServer && sceneName == "GameScene")
        {   
            GameManager.Instance.StartGame(_selectedMap); //Send map info to game manager
        }
    }

    #region Start Stuff
    private void OnEnable()
    {
        _mapDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDisable()
    {
        if(_mapDropdown != null)
        {
            _mapDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            NetworkManager.SceneManager.OnLoadComplete += SceneManager_OnLoadComplete;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
            NetworkManager.SceneManager.OnLoadComplete -= SceneManager_OnLoadComplete;

        }
    }
    private void SetDropdown()
    {
        List<string> options = new() { "Random Map" };
        foreach (MapsBase map in _maps)
        {
            options.Add(map.MapName);
            _stringToMapBase.Add(map.MapName, map);
        }

        _mapDropdown.ClearOptions();
        _mapDropdown.AddOptions(options);
    }
    private void OnDropdownValueChanged(int value)
    { 
        _currMapSelection = _mapDropdown.options[value].text.ToString();
    }
    #endregion
}
