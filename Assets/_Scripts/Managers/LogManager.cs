using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        if(dmgInfo.Target == null) { return; }
        _logTMP.text += $"\nCharacter #{dmgInfo.Source.CharacterID.Value} dealt <color=red>{damage}</color> damage to character #{dmgInfo.Target.CharacterID.Value} using {name}.";
    }

}
