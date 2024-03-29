using System.Text.RegularExpressions;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LogManager : NetworkBehaviour
{
    public static LogManager Instance;
    [SerializeField] private TextMeshProUGUI _logTMP;

    private const string _enemyIcon = "<sprite index=0> ";
    private const string _allyIcon = "<sprite index=1> ";

    public void Awake()
    {
        Instance = this;
    }

    public void LogOnSlain(AbstractCharacter character)
    {
        FixedString128Bytes log = $"#{character.CharacterID.Value} was slain.";
        SyncLogs(log);
    }
    public void LogPushPullAbility(AbstractCharacter character, CardBase card, int amtMoved, bool isPush)
    {
        string name = GetCardName(card);
        string move = isPush ? "pushed" : "pulled";
        FixedString128Bytes log = $"#{character.CharacterID.Value} was {move} {amtMoved} by <u><link={name}>{name}</link></u>.";
        SyncLogs(log);
    }

    public void LogGenericDamage(AbstractCharacter character, int damage, string source)
    {
        FixedString128Bytes log = $"#{character.CharacterID.Value} took <color=red>{damage} damage</color> from {source}.";
        SyncLogs(log);
    }

    public void LogGenericHeal(AbstractCharacter character, int heal, string source)
    {
        FixedString128Bytes log = $"#{character.CharacterID.Value} healed <color=green>{heal} health</color> from {source}.";
        SyncLogs(log);
    }

    public void LogCardReward(Class classType)
    {
        FixedString128Bytes log = $"picked a {classType} card as reward.";
        SyncLogs(log);
    }

    public void LogCardPickup(CardBase card)
    {
        string name = GetCardName(card);

        FixedString128Bytes log = $"picked up <u><link={name}>{name}</link></u>.";
        SyncLogs(log);
    }

    public void LogDrawAbility(CardBase card)
    {
        string name = GetCardName(card);

        int numCardsDrawn = DeckManager.Instance.NumOfCardsDrawn;
        if (numCardsDrawn == 0) { return; }

        string plural = numCardsDrawn == 1 ? "card" : "cards";
        FixedString128Bytes log = $"drew {numCardsDrawn} {plural} using <u><link={name}>{name}</link></u>.";
        DeckManager.Instance.NumOfCardsDrawn = 0; //reset it for next time
        SyncLogs(log);
    }

    public void LogMovementAbility(CardBase card, AbstractCharacter character ,int amount)
    {
        string name = GetCardName(card);
        FixedString128Bytes log = $"#{character.CharacterID.Value} moved {amount} hexes using <u><link={name}>{name}</link></u>.";
        SyncLogs(log);
    }

    public void LogCardDamageAbility(CardBase card, CombatInfo dmgInfo, int damage)
    {
        string name = GetCardName(card);
        if (dmgInfo.Target == null) { return; }
        
        FixedString128Bytes log = $"#{dmgInfo.Source.CharacterID.Value} dealt <color=red>{damage} damage</color> to #{dmgInfo.Target.CharacterID.Value} using <u><link={name}>{name}</link></u>.";
        SyncLogs(log);
    }

    public void LogCardHealAbility(CardBase card, CombatInfo healInfo, int heal)
    {
        string name = GetCardName(card);
        if (healInfo.Target == null) { return; }

        FixedString128Bytes log = $"#{healInfo.Source.CharacterID.Value} healed <color=green>{heal} health</color> from #{healInfo.Target.CharacterID.Value} using <u><link={name}>{name}</link></u>.";
        SyncLogs(log);
    }
    
    private string GetCharacterName(int characterId)
    {
        AbstractCharacter character = Database.Instance.AbstractCharactersDB.Get(characterId);
        string name = character.name.Replace("(Clone)", "");

        if(Database.Instance.Allies.Contains(character))
        {
            return $"<color=blue>{name}</color>"; //blue color for allies
        }
        else
        {
            return $"<color=red>{name}</color>"; //red color for enemies
        }
    }

    private string GetCardName(CardBase card)
    {
        return card.name.Replace("(Clone)", "");
    }

    #region Network Synching
    private void SyncLogs(FixedString128Bytes log)
    {
        if (IsServer) //update other client
        {
            UpdateLogManagerClientRPC(log);
            log = AddColorCharacters(log);
            _logTMP.text += _allyIcon + log + "\n";
        }
        else //Send rpc that will then update all clients
        {
            UpdateLogManagerServerRPC(log);
            log = AddColorCharacters(log);
            _logTMP.text += _allyIcon + log + "\n";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateLogManagerServerRPC(FixedString128Bytes logString)
    {
        logString = AddColorCharacters(logString);
        _logTMP.text += _enemyIcon + logString + "\n";
    }

    [ClientRpc]
    public void UpdateLogManagerClientRPC(FixedString128Bytes logString)
    {
        if(!IsServer) 
        {
            logString = AddColorCharacters(logString);
            _logTMP.text += _enemyIcon + logString + "\n";
        } 
    }

    /// <summary>
    /// Turns a '#' followed by any number into a character with the color based on ally and enemy
    /// </summary>
    /// <param name="log"></param>
    /// <returns></returns>
    private FixedString128Bytes AddColorCharacters(FixedString128Bytes log)
    {
        string logString = log.ToString();
        logString = Regex.Replace(logString, @"#(\d+)", match =>
        {
            int characterId = int.Parse(match.Groups[1].Value);
            return GetCharacterName(characterId);
        });

        log = new(logString);
        return log;
    }
    #endregion
}
