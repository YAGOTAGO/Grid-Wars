using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : NetworkBehaviour
{
    public static EndTurnButton Instance;
    public NetworkVariable<bool> IsServersTurn = new();

    private Button _button;

    private void Awake()
    {
        Instance = this;
        _button = GetComponent<Button>();
    }

    public override void OnNetworkSpawn()
    {
        _button.onClick.AddListener(OnEndTurnButtonClick);
        IsServersTurn.OnValueChanged += OnServerTurnValueChanged;
        if (IsServer)
        {
            IsServersTurn.Value = UnityEngine.Random.Range(0f, 1f) < 0.5f; //randomize who goes first
            ButtonColorUpdateClientRPC();
        }
    }

    public override void OnNetworkDespawn()
    {
        IsServersTurn.OnValueChanged -= OnServerTurnValueChanged;
    }

    public void CanClickEndTurn(bool canClick)
    {
        _button.interactable = canClick;

        if (!canClick)
        {
            _button.image.color = Color.grey;
        }
        else
        {
            ButtonColorUpdate();
        }
    }
    private void OnEndTurnButtonClick()
    {
        if (IsServer && IsServersTurn.Value) //Server and is your turn
        {
            IsServersTurn.Value = false;
        }
        else if (!IsServer && !IsServersTurn.Value)
        {
            SwapTurnServerRPC();
        }
    }

    public void OnServerTurnValueChanged(bool prevVal, bool newVal)
    {
        ButtonColorUpdate();
    }

    private void ButtonColorUpdate()
    {
        if (IsItMyTurn())
        {
            _button.image.color = Color.green;
        }
        else
        {
            _button.image.color = Color.red;
        }
    }

    public bool IsItMyTurn()
    {
        if (IsServer)
        {
            return IsServersTurn.Value;
        }
        else
        {
            return !IsServersTurn.Value;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SwapTurnServerRPC()
    {
        IsServersTurn.Value = true;
    }

    [ClientRpc]
    public void ButtonColorUpdateClientRPC()
    {
        ButtonColorUpdate();
    }
}
