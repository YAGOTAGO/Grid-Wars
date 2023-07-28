using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance;

    [Header("Tiles")]
    [SerializeField] private Tile _hoverTile;
    [SerializeField] private Tile _pathTile;

    [Header("Tile Maps")]
    [SerializeField] private Tilemap _hoverMap;
    [SerializeField] private Tilemap _movesMap;
    [SerializeField] private Tilemap _pathMap;

    private HexNode _priorNode;
    private Vector3Int previousMousePos;

    private int lol;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Highlight();
    }
    #region PathMap
    public void PathHighlight(Vector3Int pos)
    {
        _pathMap.SetTile(pos, _pathTile);
    }

    public void ClearPathAndMoves()
    {
        ClearMovesMap();
        ClearPathMap();
    }

    public void ClearPathMap()
    {
        _pathMap.ClearAllTiles();
    }
    #endregion

    //Highlights tile at grid pos
    public void MovesHighlight(Vector3Int cellPos)
    {
        _movesMap.SetTile(cellPos, _hoverTile);
    }

    //Clears all tiles
    public void ClearMovesMap()
    {
        _movesMap.ClearAllTiles();
    }


    //Highlights on hover
    private void Highlight()
    {
        HexNode curr = MouseManager.Instance.NodeMouseIsOver;
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

}
