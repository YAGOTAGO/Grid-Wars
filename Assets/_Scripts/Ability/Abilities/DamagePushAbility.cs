using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/DamagePushAbility")]
public class DamagePushAbility : AbilityBase
{
    //Variables we can set in the editor
    [SerializeField] private TargetingType _targetingType;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private int _damageAmount;
    [SerializeField] private int _pushAmount;
    [SerializeField] private Shape _shape;
    [SerializeField] private string _prompt;
    [SerializeField] private int _range;

    private AbstractShape _abstractShape;
    public override int Range { get => _range; }
    public override string Prompt => _prompt;
    public override AbstractShape Shape
    {
        get
        {
            _abstractShape ??= EnumToShape(_shape); //If abstract shape is null we set it
            return _abstractShape;
        }
        set => _abstractShape = value;
    }
    public override IEnumerator DoAbility(List<HexNode> shape, CardBase card)
    {
        foreach (HexNode node in shape)
        {
            AbstractCharacter character = node.GetCharacterOnNode();
            if (character != null)
            {
                //Damage
                DamageInfo dmgInfo = new(_damageAmount, _damageType, CardSelectionManager.Instance.SelectedCharacter, node.GetCharacterOnNode());
                int damage = DamageManager.Damage(dmgInfo);
                LogManager.Instance.LogCardDamageAbility(card, dmgInfo, damage);

                //Push
                //Have to get the direction first
                HexNode sourceNode = CardSelectionManager.Instance.SelectedCharacter.GetNodeOn();

                Vector3 displacement = sourceNode.CubeCoord.Value - node.CubeCoord.Value;
                Vector3 direction = displacement.normalized;
                Vector3Int directionInt = new(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));

                HexNode currNode = node;
                //For the push amount
                for(int i = 0; i<_pushAmount; i++)
                {
                    if(GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value - directionInt, out HexNode nextNode))
                    {
                        currNode = nextNode;
                        if (nextNode.IsNodeWalkable())
                        {
                            character.PutOnHexNode(nextNode, false);

                            //Do a tween
                            Tween characterPush = TweenManager.Instance.CharacterPush(character.gameObject, nextNode.transform.position);
                            yield return characterPush.WaitForCompletion();
                        }
                    }
                    else
                    {
                        Debug.Log("Not a valid tile in that direction to push to.");
                    }
                }

            }
            

        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
