using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private iTween.EaseType _easeType;
    private PlayerSelected _pSelect;

    private void Awake()
    {
        _pSelect = GetComponent<PlayerSelected>();
    }
    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        //Player is selected, and mouse is clicked, and tile is walkable
        if (_pSelect.IsPlayerSelected() && Input.GetMouseButtonDown(0) &&
            MouseInput.Instance.IsTileWalkable() && !_pSelect.cursorInside)
        {   
            HexNode target = GridManager.Instance.tilesDict[MouseInput.Instance.GetCellPosFromMouse(GridManager.Instance.grid)];
            HexNode start = GridManager.Instance.tilesDict[new Vector3Int(0, 0)];

            List<HexNode> path = PathFinding.FindPath(start, target);

            StartCoroutine(Walk(path));

        }
    }

    IEnumerator Walk(List<HexNode> path)
    {
        for(int i = path.Count-1; i >=0 ;i--)
        {
            MovePlayerToHex(path[i].transform.position);
            yield return new WaitForSeconds(.2f);
        }
    }

    public void MovePlayerToHex(Vector3 position)
    {

        Hashtable args = new()
        {
            ["speed"] = _playerSpeed,
            ["easetype"] = _easeType,
            ["islocal"] = true,
            ["position"] = position
        };

        iTween.MoveTo(this.gameObject, args);
    }
}
