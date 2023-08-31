using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _characterPrefabToSpawn;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //GameObject character1 = Instantiate(_characterPrefabToSpawn);
        //character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)], true);
        NetworkManager.Singleton.SceneManager.OnLoadComplete += SpawnCharacters;
    }

    public void SpawnCharacters(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        Debug.Log("Called Spawn Characters");
    }

    /*public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            GameObject character1 = Instantiate(_characterPrefabToSpawn);
            character1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)], true);
            
            GameObject character2 = Instantiate(_characterPrefabToSpawn);
            character2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            character2.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(1, 1)], true);

        }

    }*/

}
