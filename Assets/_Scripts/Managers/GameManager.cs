using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _character;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameObject character1 = Instantiate(_character);
        character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)], true);

    }


    /*public override void OnNetworkSpawn()
    {

        if (IsServer)
        {
            GameObject character1 = Instantiate(_character);
            character1.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            character1.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(0, 0)], true);
            
            GameObject character2 = Instantiate(_character);
            character2.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
            character2.GetComponent<Character>().PutOnHexNode(GridManager.Instance.GridCoordTiles[new Vector3Int(1, 1)], true);

        }
    }
*/
    /*private void Start()
    {
        GetComponent<NetworkObject>().Spawn();
    }
*/
}
