using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LogManager : NetworkBehaviour
{
    public static LogManager Instance;
    [SerializeField] private TextMeshProUGUI _logTMP;


    public void Awake()
    {
        Instance = this;
    }

    public void LogDrawAbility(CardBase card, int amount)
    {
        string name = GetCardName(card);

        int numCardsDrawn = DeckManager.Instance.NumOfCardsDrawn;
        if (numCardsDrawn == 0) { return; }

        string plural = numCardsDrawn == 1 ? "card" : "cards";
        FixedString128Bytes log = $"\n drew {numCardsDrawn} {plural} using <u><link={name}>{name}</link></u>.";
        DeckManager.Instance.NumOfCardsDrawn = 0; //reset it for next time
        SyncLogs(log);
    }

    public void LogMovementAbility(CardBase card, AbstractCharacter character ,int amount)
    {
        string name = GetCardName(card);
        FixedString128Bytes log = $"\n#{character.CharacterID.Value} moved {amount} hexes using <u><link={name}>{name}</link></u>.";
        SyncLogs(log);
    }

    public void LogDamageAbility(CardBase card, DamageInfo dmgInfo, int damage)
    {
        string name = GetCardName(card);
        if (dmgInfo.Target == null) { return; }
        
        FixedString128Bytes log = $"\n#{dmgInfo.Source.CharacterID.Value} dealt <color=red>{damage} damage</color> to #{dmgInfo.Target.CharacterID.Value} using <u><link={name}>{name}</link></u>.";
        //Debug.Log(Encoding.UTF8.GetByteCount(log.ToString()));
        SyncLogs(log);
    }

    private string GetCardName(CardBase card)
    {
        return card.name.Replace("(Clone)", "");
    }
    #region Network synching
    private void SyncLogs(FixedString128Bytes log)
    {
        if (IsServer) //update all clients (includes server)
        {
            UpdateLogManagerClientRPC(log);
        }
        else //Send rpc that will then update all clients
        {
            UpdateLogManagerServerRPC(log);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateLogManagerServerRPC(FixedString128Bytes logString)
    {
        UpdateLogManagerClientRPC(logString);
    }

    [ClientRpc]
    public void UpdateLogManagerClientRPC(FixedString128Bytes logString)
    {
        _logTMP.text += logString;
    }
    #endregion
}
