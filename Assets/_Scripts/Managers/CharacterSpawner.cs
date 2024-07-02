using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : PersistentNetworkSingleton<CharacterSpawner>
{

    public List<Character> ServerCharacters = new();
    public List<Character> ClientCharacters = new();

    private List<Vector3Int> _spawnPosServer = new();
    private List<Vector3Int> _spawnPosClient = new();

    public void SetSpawnPoints(MapsBase map)
    {
        _spawnPosServer = map.SpawnPosServer;
        _spawnPosClient = map.SpawnPosClient;
    }

    public void SpawnCharacters()
    {
        if (!IsServer) { return; }
        
        int index = 0;
        
        foreach(Character c in ServerCharacters) //Spawn Server characters
        {
            Character characterGO = Instantiate(c.gameObject).GetComponent<Character>();
            characterGO.GetComponent<NetworkObject>().SpawnWithOwnership(0); //0 is the server
            
            Character character = characterGO.GetComponent<Character>();
            character.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnPosServer[index++]], true);
        }

        index = 0;
        foreach (Character c in ClientCharacters) //Spawn client characters
        {
            Character characterGO = Instantiate(c.gameObject).GetComponent<Character>();
            characterGO.GetComponent<NetworkObject>().SpawnWithOwnership(1); //1 is the first joining client (in our case the only other player)
            
            Character character = characterGO.GetComponent<Character>();
            character.PutOnHexNode(GridManager.Instance.GridCoordTiles[_spawnPosClient[index++]], true);
        }

    }


}
