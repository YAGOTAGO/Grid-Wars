using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public NetworkVariable<bool> IsServersTurn = new();

    [SerializeField] private Button _endTurnButton;

    private void Awake()
    {
        Instance = this;
        _endTurnButton.onClick.AddListener(OnEndTurnButtonClick);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            IsServersTurn.Value = UnityEngine.Random.Range(0f, 1f) < 0.5f; //randomize who goes first
        }
    }
    private void OnEndTurnButtonClick()
    {
        if (IsServer && IsServersTurn.Value) //Server and is your turn
        {
            IsServersTurn.Value = false;
        }
        else if(!IsServer && !IsServersTurn.Value)
        {
            SwapTurnServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SwapTurnServerRPC()
    {
        IsServersTurn.Value = true;
    }

    private void Update()
    {
        
    }
    
}
