using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public static MouseInput Instance;

    private void Awake()
    {
        Instance= this;
    }

    public bool IsTileWalkable()
    {
        Vector3Int pos = GetCellPosFromMouse();
        if(GridManager.Instance.tilesDict.ContainsKey(pos))
        {
            return GridManager.Instance.tilesDict[pos].isWalkable;
        }

        return false;
    }

    //Returns node that mouse is over or null if none there
    public HexNode GetNodeFromMouse()
    {
        HexNode value;
        Vector3Int gridPos = GetCellPosFromMouse();
        if(GridManager.Instance.tilesDict.TryGetValue(gridPos, out value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public Vector3Int GetCellPosFromMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return GridManager.Instance.grid.WorldToCell(mouseWorldPos);
    }
}
