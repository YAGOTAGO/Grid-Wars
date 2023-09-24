using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TweenManager : NetworkBehaviour
{
    public static TweenManager Instance;

    [Header("Card Move")]
    [SerializeField][Range(0, 1)] private float _cardDuration;
    [SerializeField] private Ease _cardMoveEase;
    
    [Header("Card Scale")]
    [SerializeField][Range(0, 3)] private float _cardScaleUp;
    [SerializeField][Range(0, 3)] private float _cardScaleDown;
    [SerializeField][Range(0, 3)] private float _cardScaleDuration;
    [SerializeField] private Ease _cardScaleEase;
        
    [Header("Character Movement")]
    [SerializeField] private Ease _characterMoveEase;
    [SerializeField] private Ease _characterPushEase;
    [SerializeField] private Ease _characterDashEase;
    [SerializeField][Range(0, 1)] private float _characterPushDuration;
    [SerializeField][Range(0, 1)] private float _characterMoveDuration;
    [SerializeField][Range(0, 1)] private float _characterDashDuration;

    private void Awake()
    {
        Instance = this;
    }

    public Tween CardMove(GameObject card, Vector3 target)
    {
        return card.transform.DOMove(target, _cardDuration).SetEase(_cardMoveEase);
    }

    public Tween CardScale(GameObject card, bool scaleUp)
    {
        float scale = scaleUp? _cardScaleUp : _cardScaleDown;
        return card.transform.DOScale(new Vector3(scale, scale), _cardScaleDuration).SetEase(_cardScaleEase);
    }

    public void SetCardDefaultSize(Transform card)
    {
        card.localScale = new Vector3(_cardScaleDown, _cardScaleDown, _cardScaleDown);
    }

    public Tween CharacterDash(GameObject character, Vector3 target)
    {
        if (IsServer)
        {
            return character.transform.DOMove(target, _characterDashDuration).SetEase(_characterDashEase);
        }
        else
        {
            CharacterMoveServerRPC(character.GetComponent<Character>().CharacterID.Value, target);
            return character.transform.DOMove(target, _characterDashDuration).SetEase(_characterDashEase);
        }
    }

    public Tween CharacterPushOrPull(GameObject character, Vector3 target)
    {
        if (IsServer)
        {
            return character.transform.DOMove(target, _characterPushDuration).SetEase(_characterPushEase);
        }
        else
        {
            CharacterMoveServerRPC(character.GetComponent<Character>().CharacterID.Value, target);
            return character.transform.DOMove(target, _characterPushDuration).SetEase(_characterPushEase);
        }
    }

    public Tween CharacterMove(GameObject character, Vector3 target)
    {
        if (IsServer)
        {
            return character.transform.DOMove(target, _characterMoveDuration).SetEase(_characterMoveEase);
        }
        else
        {
            CharacterMoveServerRPC(character.GetComponent<Character>().CharacterID.Value, target);
            return character.transform.DOMove(target, _characterMoveDuration).SetEase(_characterMoveEase);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void CharacterMoveServerRPC(int characterID, Vector3 target)
    {
        Database.Instance.CharactersDB.Get(characterID).gameObject.transform.DOMove(target, _characterMoveDuration).SetEase(_characterMoveEase);
    }
}
