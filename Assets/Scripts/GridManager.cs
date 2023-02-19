using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    public static GridManager Instance;

    public Vector3Int cellPos { get; private set; }
    public Grid grid { get; private set; }
    [SerializeField] private Tilemap gameTileMap;
    [SerializeField] private Tilemap highlightMap;
    [SerializeField] private Tile hoverTile;

    private Vector3Int previousMousePos = new();
    private GameRuleTile gameTile;

    private void Awake()
    {
        Instance = this;
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {

        cellPos = GetCellPosAtMousePos();
        gameTile = (GameRuleTile)gameTileMap.GetTile(cellPos);
        
        //Highlight tile logic
        HighlightTiles(cellPos);
       
    }

        
    private void HighlightTiles(Vector3Int mousePos)
    {
        //Check if moved
        if (!mousePos.Equals(previousMousePos))
        {
            //Check if there is a tile there
            if (gameTileMap.HasTile(mousePos))
            {
                highlightMap.SetTile(previousMousePos, null); // Remove old hoverTile
                highlightMap.SetTile(mousePos, hoverTile);
                previousMousePos = mousePos;
            }
            else
            {
                highlightMap.SetTile(previousMousePos, null);
            }

        }
    }
    
    public Vector3 GetTileAtMousePos()
    {
        Vector3Int gridPos = GetCellPosAtMousePos();
        return gameTileMap.GetCellCenterLocal(gridPos);
    }

    public Vector3Int GetCellPosAtMousePos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return grid.WorldToCell(mouseWorldPos);
    }



}
