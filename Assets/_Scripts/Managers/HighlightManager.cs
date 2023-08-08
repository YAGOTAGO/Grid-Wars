using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance;

    [Header("Tiles")]
    [SerializeField] private Tile _hoverTile;
    [SerializeField] private Tile _targetTile;

    [Header("Tile Maps")]
    [SerializeField] private Tilemap _hoverMap;
    [SerializeField] private Tilemap _rangeMap;
    [SerializeField] private Tilemap _targetMap;

    //For highlighting
    private HexNode _priorNode;

    private void Awake(){ Instance = this; }

    private void Update()
    {
        Highlight();
    }

    public void ClearTargetAndRange()
    {
        ClearRangeMap();
        ClearTargetMap();
    }

    public void ClearTargetMap()
    {
        _targetMap.ClearAllTiles();
    }

    //Highlights tile at grid pos
    public void RangeHighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, _hoverTile);
    }

    public void RangeUnhighlight(Vector3Int cellPos)
    {
        _rangeMap.SetTile(cellPos, null);
    }
    public void TargetHighlight(Vector3Int pos)
    {
        _targetMap.SetTile(pos, _targetTile);
    }

    //Clears all tiles
    public void ClearRangeMap()
    {
        _rangeMap.ClearAllTiles();
    }

    //Highlights on hover
    private void Highlight()
    {
        HexNode curr = MouseManager.Instance.NodeMouseIsOver;
        if(curr == null) { return; }
        Vector3Int currGridPos = curr.GridPos;

        if (GridManager.Instance.GridCoordTiles.ContainsKey(currGridPos))
        {
            //Check if moved
            if (!curr.Equals(_priorNode))
            {
                if(_priorNode != null){ _hoverMap.SetTile(_priorNode.GridPos, null); }// Remove old hoverTile
                _hoverMap.SetTile(currGridPos, _hoverTile);
                _priorNode = curr;
            }

        }
    }

    public void HighlightRangeSet(HashSet<HexNode> visited)
    {
        foreach (HexNode node in visited)
        {
            RangeHighlight(node.GridPos);
        }
    }

    public void HighlightTargetList(List<HexNode> visited)
    {
        foreach(HexNode node in visited)
        {
            TargetHighlight(node.GridPos);
        }
    }

}
