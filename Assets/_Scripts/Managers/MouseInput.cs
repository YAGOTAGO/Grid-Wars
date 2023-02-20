using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public static MouseInput Instance;
    public int mousePos { get; private set; }

    private void Awake()
    {
        Instance= this;
    }

    public bool IsTileWalkable()
    {
        Vector3Int pos = GetCellPosFromMouse(GridManager.Instance.grid);
        if(GridManager.Instance.tilesDict.ContainsKey(pos))
        {
            return GridManager.Instance.tilesDict[pos].isWalkable;
        }

        return false;
    }

    public Vector3Int GetCellPosFromMouse(Grid grid)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        return grid.WorldToCell(mouseWorldPos);
    }
}
