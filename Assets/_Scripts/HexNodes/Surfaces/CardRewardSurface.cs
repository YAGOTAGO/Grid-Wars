using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/CardRewardSurface")]
public class CardRewardSurface : SurfaceBase
{
    private bool _isWalkable = true;
    private bool _canAbilitiesPassthrough = true;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private Sprite _cardRewardIcon;

    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;
    public override Sprite SurfaceSprite => _cardRewardIcon;

    public override void OnEnterNode(AbstractCharacter character)
    {
        CardRewardScreen.Instance.PickThreeCards(_rarity);
        character.GetNodeOn().SetSurface(Database.Instance.GetSurface("EmptySurface")); //remove this surface on touch
        character.GetNodeOn().SetSurfaceWalkable(false); //make new surface not walkable

        LogManager.Instance.LogCardReward(_rarity);
    }
}
