using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardSurface : SurfaceBase
{
    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _canAbilitiesPassthrough;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private Sprite _cardRewardIcon;

    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    public override Sprite SurfaceSprite => _cardRewardIcon;

    public override void OnTouchNode(AbstractCharacter character)
    {
        CardRewardScreen.Instance.PickThreeCards(_rarity);
    }
}
