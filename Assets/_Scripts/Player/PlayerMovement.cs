using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSelected))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private iTween.EaseType _easeType;
    private PlayerSelected _pSelect;
    private HexNode _onNode;
    private int moves = 5;
    private HashSet<HexNode> _possMoves = new();
    private bool _firstTime = true;

    private void Awake()
    {
        _pSelect = GetComponent<PlayerSelected>();
    }

    private void Update()
    {
        if (_pSelect.IsPlayerSelected() && _firstTime)
        {
            _possMoves = BFS.BFSvisited(_onNode, moves);
            _firstTime = false;
        }else if (!_pSelect.IsPlayerSelected())
        {
            HighlightManager.Instance.ClearMovesMap();
            _firstTime = true;
        }

        Move();

    }

    private void Start()
    {
        _onNode = GridManager.Instance.tilesDict[new Vector3Int(0, 0, 0)];
    }

    private void Move()
    {
        //Player is selected, and mouse is clicked, and tile is walkable
        if (Input.GetMouseButtonDown(0) && MouseInput.Instance.IsTileWalkable() 
            && !_pSelect.cursorInside)
        {   
            //What we clicked after selecting player
            HexNode target = GridManager.Instance.tilesDict[MouseInput.Instance.GetCellPosFromMouse(GridManager.Instance.grid)];

            //Check that we have enough moves to make it
            if(_possMoves.Contains(target))
            {
                //Clear the highlighted map
                HighlightManager.Instance.ClearMovesMap();

                //A* find path and then walk it
                List<HexNode> path = PathFinding.FindPath(_onNode, target);
                StartCoroutine(Walk(path));
                _onNode = target;
                _firstTime=true;
            }
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
