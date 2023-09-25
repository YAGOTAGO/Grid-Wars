using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/CardPickupSurface")]
public class CardPickupSurface : SurfaceBase
{
    private bool _isWalkable = true;
    private bool _canAbilitiesPassthrough = true;
    [SerializeField] private Rarity _rarity;

    private CardBase _card;

    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }

    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;

    public override Sprite SurfaceSprite 
    {
        get
        {
            if(_card == null) 
            { 
                switch(_rarity)
                {
                    case Rarity.COMMON: _card = Database.Instance.GetRandomCommonCard(); break;
                    case Rarity.RARE: _card = Database.Instance.GetRandomRareCard(); break;
                }
            }
            return _card.IconArt;
        }
    }


    public override void OnEnterNode(AbstractCharacter character)
    {
        //queue up and animation here or something
        DeckManager.Instance.AddToDeck(_card);
        character.GetNodeOn().SetSurface(Database.Instance.GetSurface("EmptySurface")); //remove this surface on touch
        character.GetNodeOn().SetSurfaceWalkable(false); //make new surface not walkable

        LogManager.Instance.LogCardPickup(_card);
    }
}
