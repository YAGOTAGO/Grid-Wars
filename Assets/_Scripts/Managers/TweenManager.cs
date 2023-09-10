using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TweenManager : NetworkBehaviour
{
    public static TweenManager Instance;

    [Header("Card Move")]
    [Range(500, 900)] public float CardSpeed;
    public Ease CardMoveEase;
    
    [Header("Card Scale")]
    [Range(0, 3)] public float CardScaleUp;
    [Range(0, 3)] public float CardScaleDown;
    [Range(0, 3)] public float CardScaleDuration;
    public Ease CardScaleEase;

    [Header("Character Movement")]
    public Ease CharacterMoveEase;
    public Ease CharacterPushEase;
    public Ease CharacterDashEase;
    [Range(0, 5)] public float CharacterPushSpeed;
    [Range(0, 5)] public float CharacterMoveSpeed;
    [Range(0, 5)] public float CharacterDashSpeed;


    private void Awake()
    {
        Instance = this;
    }

    public Tween CardMove(GameObject card, Vector3 target)
    {
        return card.transform.DOMove(target, CardSpeed).SetSpeedBased(true).SetEase(CardMoveEase);
    }

    public Tween CardScale(GameObject card, float scale)
    {
        return card.transform.DOScale(new Vector3(scale, scale), CardScaleDuration).SetEase(CardScaleEase);
    }

    public Tween CharacterDash(GameObject character, Vector3 target)
    {
        if (IsServer)
        {
            return character.transform.DOMove(target, CharacterDashSpeed).SetSpeedBased(true).SetEase(CharacterDashEase);
        }
        else
        {
            CharacterMoveServerRPC(character.GetComponent<Character>().CharacterID.Value, target);
            return character.transform.DOMove(target, CharacterDashSpeed).SetSpeedBased(true).SetEase(CharacterDashEase);
        }
    }

    public Tween CharacterPushOrPull(GameObject character, Vector3 target)
    {
        if (IsServer)
        {
            return character.transform.DOMove(target, CharacterPushSpeed).SetSpeedBased(true).SetEase(CharacterPushEase);
        }
        else
        {
            CharacterMoveServerRPC(character.GetComponent<Character>().CharacterID.Value, target);
            return character.transform.DOMove(target, CharacterPushSpeed).SetSpeedBased(true).SetEase(CharacterPushEase);
        }
    }

    public Tween CharacterMove(GameObject character, Vector3 target)
    {
        if (IsServer)
        {
            return character.transform.DOMove(target, CharacterMoveSpeed).SetSpeedBased(true).SetEase(CharacterMoveEase);
        }
        else
        {
            CharacterMoveServerRPC(character.GetComponent<Character>().CharacterID.Value, target);
            return character.transform.DOMove(target, CharacterMoveSpeed).SetSpeedBased(true).SetEase(CharacterMoveEase);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void CharacterMoveServerRPC(int characterID, Vector3 target)
    {
        Database.Instance.PlayerCharactersDB.Get(characterID).gameObject.transform.DOMove(target, CharacterMoveSpeed).SetSpeedBased(true).SetEase(CharacterMoveEase);
    }
}
