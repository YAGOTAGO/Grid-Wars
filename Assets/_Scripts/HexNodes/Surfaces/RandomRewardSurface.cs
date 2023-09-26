using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Surface", menuName = "Surfaces/RandomRewardSurface")]
public class RandomRewardSurface : SurfaceBase
{
    private Class _classType;
    private bool _isWalkable = true;
    private bool _canAbilitiesPassthrough = true;
    private Sprite _surfaceSprite;
    private int ID => (int)NodeOn.NetworkObject.NetworkObjectId;
    
    protected Class ClassType
    {
        get
        {
            if (!SurfaceSync.Instance.ContainsID(ID))
            {
                int random = Random.Range(0, PlayerSpawner.Instance.CharacterList.Count);
                Class characterClass = PlayerSpawner.Instance.CharacterList[random].CharacterClass;
                SurfaceSync.Instance.SetClass(ID, characterClass);
                _classType = characterClass;
            }
            else
            {
                _classType = SurfaceSync.Instance.GetClass(ID);
            }
            
            return _classType;
        }
    }

    public override bool IsWalkable { get => _isWalkable; set => _isWalkable = value; }
    public override bool CanAbilitiesPassthrough => _canAbilitiesPassthrough;

    public override Sprite SurfaceSprite 
    {
        get
        {
            if (_surfaceSprite == null)
            {
                _surfaceSprite = PlayerSpawner.Instance.CharacterList.FirstOrDefault(character=> character.CharacterClass == ClassType).RewardIcon;
            }
            
            return _surfaceSprite;
        } 
    }

    public override void OnEnterNode(AbstractCharacter character)
    {
        if (EndTurnButton.Instance.IsItMyTurn()) //only pick card if is your turn
        {
            CardRewardScreen.Instance.PickThreeCards(ClassType);
            LogManager.Instance.LogCardReward(ClassType);
        }

        character.GetNodeOn().SetSurface(Database.Instance.GetSurfaceByName("EmptySurface")); //remove this surface on touch
        character.GetNodeOn().SetSurfaceWalkable(false); //make new surface not walkable
    }
}
