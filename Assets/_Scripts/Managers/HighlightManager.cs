using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : MonoBehaviour
{
    public static HighlightManager Instance;

    [SerializeField] private Tile _hoverTile;
    [SerializeField] private Tilemap _hoverMap;
    [SerializeField] private Tilemap _movesMap;

    private Vector3Int previousMousePos;

    private void Awake()
    {
        Instance = this;
    }
    

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

    //Works by setting a highlight in tilemap that is over gametile map
    public void HoverHighlight(Vector3Int cellPos)
    {
        //Check if moved
        if (!cellPos.Equals(previousMousePos))
        {
            _hoverMap.SetTile(previousMousePos, null); // Remove old hoverTile
            _hoverMap.SetTile(cellPos, _hoverTile);
            previousMousePos = cellPos;
            
        }
    }
}
