using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/RandomRewardSurface")]
public class RandomRewardSurface : SurfaceBase
{
    [Header("Icons")]
    [SerializeField] private Sprite _commonRewardIcon;
    [SerializeField] private Sprite _rareRewardIcon;
    [SerializeField] private Sprite _epicRewardIcon;

    [Header("Distributions")]
    [Range(0, 100)]
    [SerializeField] private int _commonChance;
    [Range(0, 100)]
    [SerializeField] private int _rareChance;

    private bool _isWalkable = true;
    private bool _canAbilitiesPassthrough = true;
    private Rarity _rarity;
    private int ID => (int)NodeOn.NetworkObject.NetworkObjectId;

    private void OnDestroy()
    {
        SurfaceSync.Instance.RemoveRarity(ID);
    }
    
    protected Rarity Rarity
    {
        get
        {
            if (!SurfaceSync.Instance.ContainsID(ID))
            {
                int random = Random.Range(1, 101);
                if(random <= _commonChance)
                {
                    SurfaceSync.Instance.SetRarity(ID, Rarity.COMMON);
                    _rarity = Rarity.COMMON;
                    return _rarity;
                }
                if(random <= (_commonChance + _rareChance))
                {
                    SurfaceSync.Instance.SetRarity(ID, Rarity.RARE);
                    _rarity = Rarity.RARE;
                    return _rarity;
                }
                if (random <= 100)
                {
                    SurfaceSync.Instance.SetRarity(ID, Rarity.EPIC);
                    _rarity = Rarity.EPIC;
                    return _rarity;
                }
            }
            else
            {
                _rarity = SurfaceSync.Instance.GetRarity(ID);
            }
            return _rarity;
        }
    }

    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;

    private Sprite _surfaceSprite;
    public override Sprite SurfaceSprite 
    {
        get
        {
            if (_surfaceSprite == null)
            {
                switch (Rarity)
                {
                    case Rarity.COMMON: _surfaceSprite = _commonRewardIcon; break;
                    case Rarity.RARE: _surfaceSprite = _rareRewardIcon; break;
                    case Rarity.EPIC: _surfaceSprite = _epicRewardIcon; break;
                }
            }
            
            return _surfaceSprite;
        } 
    }

    public override void OnEnterNode(AbstractCharacter character)
    {
        CardRewardScreen.Instance.PickThreeCards(Rarity);
        character.GetNodeOn().SetSurface(Database.Instance.GetSurface("EmptySurface")); //remove this surface on touch
        character.GetNodeOn().SetSurfaceWalkable(false); //make new surface not walkable

        LogManager.Instance.LogCardReward(Rarity);
    }
}
