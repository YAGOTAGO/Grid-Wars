using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public static PlayerSpawner Instance;
    public List<Character> CharacterList = new(); //list of characters
    [SerializeField] private List<Vector3Int> _spawnLocations = new();

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnCharacters()
    {
        int positionIndex = 0;
       
        if (IsServer)
        {
            foreach(ulong clientId in NetworkManager.ConnectedClientsIds)
            {
                GameObject monkGO = Instantiate(CharacterList[0].gameObject);
                GameObject clericGO = Instantiate(CharacterList[1].gameObject);
                GameObject wizardGO = Instantiate(CharacterList[2].gameObject);

                Character monkCharacter = monkGO.GetComponent<Character>();
                Character clericCharacter = clericGO.GetComponent<Character>();
                Character wizardCharacter = wizardGO.GetComponent<Character>();

                monkGO.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                clericGO.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                wizardGO.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

                monkCharacter.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                clericCharacter.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                wizardCharacter.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnLocations[positionIndex++]], true);
                    
            }

        }


    }



}
