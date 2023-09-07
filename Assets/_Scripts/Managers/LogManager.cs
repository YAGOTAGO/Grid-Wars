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

    public void LogDrawAbility(string abilityName, int amount)
    {
        string name = abilityName.Replace("Ability", "");
        FixedString128Bytes log = $"\n drew {amount} cards using {name}.";
        SyncLogs(log);
    }

    public void LogMovementAbility(string abilityName, AbstractCharacter character ,int amount)
    {
        string name = abilityName.Replace("Ability", "");
        FixedString128Bytes log = $"\n#{character.CharacterID.Value} moved {amount} hexes using {name}.";
        SyncLogs(log);
    }

    public void LogDamageAbility(string abilityName, DamageInfo dmgInfo, int damage)
    {
        string name = abilityName.Replace("Ability", "");
        if (dmgInfo.Target == null) { return; }
        
        FixedString128Bytes log = $"\n#{dmgInfo.Source.CharacterID.Value} dealt <color=red>{damage} damage</color> to #{dmgInfo.Target.CharacterID.Value} using <u><link={name}>{name}</link></u>.";
        //Debug.Log(Encoding.UTF8.GetByteCount(log.ToString()));
        SyncLogs(log);
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
