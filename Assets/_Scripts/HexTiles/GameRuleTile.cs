using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tile/RuleTile")]
public class GameRuleTile : HexagonalRuleTile
{   

    [SerializeField] public TileType type;

}


