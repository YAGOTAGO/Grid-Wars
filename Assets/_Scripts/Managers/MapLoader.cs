using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : NetworkBehaviour
{

    public static MapLoader Instance { get; private set; }

    [SerializeField] private List<MapsBase> _maps = new();
    [SerializeField] private TMP_Dropdown _mapDropdown;

    private Dictionary<string, MapsBase> _stringToMapBase = new();
    private string _currMapSelection = "Random";
    
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        _mapDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _mapDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int value)
    { 
        _currMapSelection = _mapDropdown.options[value].text.ToString();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Server is responsible for calling the start of the game
        if (IsServer && scene.name == "GameScene")
        {
            Unity.Mathematics.Random rnd = new();
            MapsBase map = _currMapSelection == "Random" ? _stringToMapBase.Values.ElementAt(rnd.NextInt(0, _stringToMapBase.Count)) : _stringToMapBase[_currMapSelection];

            if(map == null) { Debug.LogError("MAP NOT FOUND IN MAP LOADER!"); }

            //Send an RPC to begin wait for hex neighboors


            //Send map and Character info to game manager
            GameManager.Instance.StartGame(map);
            
        }

    }

    public IEnumerator SpawnCharacters()
    {
       yield return new WaitUntil(() => GridManager.Instance.MapLoaded);
       
    }


}
