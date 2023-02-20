using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class OldGridMang : MonoBehaviour
{

    public static OldGridMang Instance;

    public Vector3Int cellPos { get; private set; }
    public Grid grid { get; private set; }
    [SerializeField] private Tilemap gameTileMap;
    private Dictionary<Vector3Int, GameRuleTile> tiles; //used to keep position and tile data
    [SerializeField] private Highlight highlightScript;


    private void Awake()
    {
        Instance = this;
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {

        //Get cell pos based on mouse
        cellPos = GetCellPosAtMousePos();
        
        //Highlight tile logic at cell
        if(gameTileMap.HasTile(cellPos))
            highlightScript.HighlightTiles(cellPos);
       
    }
    
    private void InitMap(Tilemap tilemap)
    {
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (gameTileMap.HasTile(position))
            {
                
                tilemap.SetTile(position, null);
                tilemap.SetTile(position, Instantiate(new GameRuleTile()));

            }
            
        }
    }


 /*   //Works by setting a highlight in tilemap that is over gametile map
    private void HighlightTiles(Vector3Int cellPos)
    {
        //Check if moved
        if (!cellPos.Equals(previousMousePos))
        {
            //Check if there is a tile there
            if (gameTileMap.HasTile(cellPos))
            {   
                highlightMap.SetTile(previousMousePos, null); // Remove old hoverTile
                highlightMap.SetTile(cellPos, hoverTile);
                previousMousePos = cellPos;
                Debug.Log(cellPos);
            }
            else
            {
                highlightMap.SetTile(previousMousePos, null);
            }

        }
    }*/

    //Returns whether the tile that mouse is over is walkable
    /*public bool isTileWalkable()
    {
        Vector3Int gridPos = GetCellPosAtMousePos();
        return gameTileMap.GetTile<GameRuleTile>(gridPos).isWalkable;
    }*/
    
    //Returns the local vector 3 of tile that mouse is over
    public Vector3 GetLocalPosFromMousePos()
    {
        Vector3Int gridPos = GetCellPosAtMousePos();
        return gameTileMap.GetCellCenterLocal(gridPos);
    }

    //Returns the cell position of where mouse is
    public Vector3Int GetCellPosAtMousePos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return grid.WorldToCell(mouseWorldPos);
    }

    //Return the tile that player is standing on
    public GameRuleTile GetTileFromTransform(Transform transform)
    {
        Vector3Int gridPos = grid.WorldToCell(transform.position);
        return gameTileMap.GetTile<GameRuleTile>(gridPos);
    }


  /*  public int GetDistance(GameRuleTile tileA, GameRuleTile tileB)
    {
        

        int dx = aX2 - aX1;     // signed deltas
        int dy = aY2 - aY1;
        int x = Mathf.Abs(dx);  // absolute deltas
        int y = Mathf.Abs(dy);
        // special case if we start on an odd row or if we move into negative x direction
        if ((dx < 0) ^ ((aY1 & 1) == 1))
            x = Mathf.Max(0, x - (y + 1) / 2);
        else
            x = Mathf.Max(0, x - (y) / 2);
        return x + y;
    }*/

}
