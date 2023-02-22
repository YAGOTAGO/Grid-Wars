using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    private void Awake()
    {
        Instance= this;
        DontDestroyOnLoad(this);
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
