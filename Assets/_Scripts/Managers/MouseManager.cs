using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;
    public Vector3Int MouseCellPos { get; private set; } //coords of node ie: 0,1

    #region Singletons
    private GridManager _gridManager;

    #endregion

    void Awake()
    {
        Instance= this;
        MouseCellPos = new Vector3Int();
        DontDestroyOnLoad(this);
    }
     void Start()
    {
        _gridManager = GridManager.Instance;
    }
    void Update()
    {
        MouseCellPos = GetCellPosFromMouse();
    }
    //Returns whethet hovered tile is walkable
    public bool IsTileWalkable()
    {
        if(_gridManager.GridCoordTiles.ContainsKey(MouseCellPos))
        {
            return _gridManager.GridCoordTiles[MouseCellPos].IsWalkable;
        }

        return false;
    }

    //Returns node that mouse is over or null if none there
    public HexNode GetNodeFromMouse()
    {

        if (_gridManager.GridCoordTiles.TryGetValue(MouseCellPos, out HexNode value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    private Vector3Int GetCellPosFromMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return _gridManager.Grid.WorldToCell(mouseWorldPos);
    }
}
