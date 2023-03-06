using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;
    public Vector3Int MouseCellPos { get; private set; } //coords of node ie: 0,1

    private void Awake()
    {
        Instance= this;
        MouseCellPos = new Vector3Int();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        MouseCellPos = GetCellPosFromMouse();
    }
    //Returns whethet hovered tile is walkable
    public bool IsTileWalkable()
    {
        if(GridManager.Instance.TilesDict.ContainsKey(MouseCellPos))
        {
            return GridManager.Instance.TilesDict[MouseCellPos].isWalkable;
        }

        return false;
    }

    //Returns node that mouse is over or null if none there
    public HexNode GetNodeFromMouse()
    {

        if (GridManager.Instance.TilesDict.TryGetValue(MouseCellPos, out HexNode value))
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
        return GridManager.Instance.Grid.WorldToCell(mouseWorldPos);
    }
}
