using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public NetworkVariable<bool> IsServersTurn = new();

    [SerializeField] private Button _endTurnButton;
    [SerializeField] private GameObject _popUpGO;
    [SerializeField] private TextMeshProUGUI _popUpTMP;
    private Coroutine _popUpCoroutine;

    private void Awake()
    {
        Instance = this;
        _endTurnButton.onClick.AddListener(OnEndTurnButtonClick);
        _popUpGO.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        IsServersTurn.OnValueChanged += OnServerTurnValueChanged;
        if (IsServer)
        {
            IsServersTurn.Value = UnityEngine.Random.Range(0f, 1f) < 0.5f; //randomize who goes first
            ButtonColorUpdateClientRPC();
        }
    }
    
    public bool IsAlly(AbstractCharacter character)
    {
        return character.IsOwner;
    }

    public void PopUpText(string text)
    {
        if(_popUpCoroutine != null)
        {
            StopCoroutine(_popUpCoroutine);
        }
        
        _popUpCoroutine = StartCoroutine(PopUpCoroutine(text));

    }

    private IEnumerator PopUpCoroutine(string text)
    {
        _popUpGO.SetActive(true);
        _popUpTMP.text = text;
          
        yield return new WaitForSeconds(3f);

        _popUpGO.SetActive(false);
    }

    public void CanClickEndTurn(bool canClick)
    {
        _endTurnButton.interactable = canClick;
        
        if (!canClick)
        {
            _endTurnButton.image.color = Color.grey;
        }
        else
        {
            ButtonColorUpdate();
        }
    }

    [ClientRpc]
    private void ButtonColorUpdateClientRPC()
    {
        ButtonColorUpdate();
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
    
    private void ButtonColorUpdate()
    {
        if (IsItMyTurn())
        {
            _endTurnButton.image.color = Color.green;
        }
        else
        {
            _endTurnButton.image.color = Color.red;
        }
    }
    private void OnServerTurnValueChanged(bool prevVal,  bool newVal)
    {
        ButtonColorUpdate();
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
    
}
