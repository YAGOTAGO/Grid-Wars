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

    private int _amountPushed = 0;

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

                //For the push amount
                HexNode currNode = node;
                for (int i = 0; i<_pushAmount; i++)
                {
                    if(GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value - directionInt, out HexNode nextNode))
                    {
                        currNode = nextNode;
                        if (nextNode.IsNodeWalkable())
                        {
                            character.PutOnHexNode(nextNode, false);

                            _amountPushed++;

                            //Do a tween
                            Tween characterPush = TweenManager.Instance.CharacterPush(character.gameObject, nextNode.transform.position);
                            yield return characterPush.WaitForCompletion();
                        }
                    }
                }

                LogManager.Instance.LogPushAbility(character, card, _amountPushed);
                _amountPushed = 0;
            }
           
        }
        yield break;
    }

    public override TargetingType GetTargetingType()
    {
        return _targetingType;
    }
}
