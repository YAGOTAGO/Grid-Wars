using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Highlight : MonoBehaviour
{
    [SerializeField] private Tile hoverTile;
    private Tilemap highlightMap;
    private Vector3Int previousMousePos;

    private void Awake()
    {
        highlightMap = GetComponent<Tilemap>();
    }

    //Works by setting a highlight in tilemap that is over gametile map
    public void HighlightTiles(Vector3Int cellPos)
    {
        //Check if moved
        if (!cellPos.Equals(previousMousePos))
        {
            highlightMap.SetTile(previousMousePos, null); // Remove old hoverTile
            highlightMap.SetTile(cellPos, hoverTile);
            previousMousePos = cellPos;
            
        }
    }
}
