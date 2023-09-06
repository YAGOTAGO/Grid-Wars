using System.Collections;
using System.Collections.Generic;
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

    public void LogDamageAbility(string abilityName, DamageInfo dmgInfo, int damage)
    {
        string name = abilityName.Replace("Ability", "");
        if (dmgInfo.Target == null) { return; }
        
        FixedString128Bytes log = $"\nCharacter #{dmgInfo.Source.CharacterID.Value} dealt <color=red>{damage} damage</color> to character #{dmgInfo.Target.CharacterID.Value} using {name}.";

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

}
