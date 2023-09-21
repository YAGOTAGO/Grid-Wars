using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : NetworkBehaviour
{
    public static HighlightManager Instance;

    [Header("Tiles")]
    [SerializeField] private Tile _hoverTile;
    [SerializeField] private Tile _targetTile;

    [Header("Tile Maps")]
    [SerializeField] private Tilemap _hoverMap;
    [SerializeField] private Tilemap _rangeMap;
    [SerializeField] private Tilemap _targetMap;

    private void Awake(){ Instance = this; }

    public void ClearTargetAndRange()
    {
        ClearRangeMap();
        ClearTargetMap();
    }

    public void ClearTargetMap()
    {
        _targetMap.ClearAllTiles();
    }
    private void ClearRangeMap()
    {
        _rangeMap.ClearAllTiles();
    }

    #region character highlighting
    public void CharacterHighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, _hoverTile);
    }

    public void CharacterUnhighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, null);
    }
    #endregion

    //Highlights tile at grid pos
    private void RangeHighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, _hoverTile);
    }
        
    private void TargetHighlight(Vector3Int pos)
    {
        _targetMap.SetTile(pos, _targetTile);
    }
    
    public void HighlightHover(HexNode hex, bool highlight)
    {
        Vector3Int currGridPos = hex.GridPos.Value;

        if (highlight)
        {
            _hoverMap.SetTile(currGridPos, _hoverTile);
        }
        else
        {
            _hoverMap.SetTile(currGridPos, null);
        }
    }

    public void HighlightRangeList(List<HexNode> visited)
    {
        foreach (HexNode node in visited)
        {
            RangeHighlight(node.GridPos.Value);
        }
    }

    public void HighlightTargetList(List<HexNode> visited)
    {
        foreach(HexNode node in visited)
        {
            TargetHighlight(node.GridPos.Value);
        }
    }

}
