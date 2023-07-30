using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenManager : MonoBehaviour
{
    public static TweenManager Instance;

    [Header("Card Move")]
    [Range(400, 700)] public float CardSpeed;
    public Ease CardMoveEase;
    
    [Header("Card Scale")]
    [Range(0, 3)] public float CardScaleUp;
    [Range(0, 3)] public float CardScaleDuration;
    public Ease CardScaleEase;

    [Header("Character Movement")]
    public Ease CharacterMoveEase;
    [Range(0, 10)] public float CharacterMoveSpeed;

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

    public Tween CharacterMove(GameObject character, Vector3 target)
    {
        return character.transform.DOMove(target, CharacterMoveSpeed).SetSpeedBased(true).SetEase(CharacterMoveEase);
    }
}