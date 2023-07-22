using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (_gridManager.GridCoordTiles.TryGetValue(MouseCellPos, out HexNode value))
        {
            return value.IsWalkable;
        }

        return false;
    }

    //Returns node that mouse is over or null if none there
    public HexNode GetNodeFromMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject()){ return null;}

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
        if (EventSystem.current.IsPointerOverGameObject()) { return new Vector3Int(0 , 0, 100); } //this value would never be a cell

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return _gridManager.Grid.WorldToCell(mouseWorldPos);
    }
}
