using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSelected))]
public class PlayerMovement : MonoBehaviour
{

    private HexNode _onNode;
    [SerializeField] private HashSet<HexNode> _possMoves = new();
    private readonly HashSet<HexNode> _emptySet = new();
    private PlayerSelected _pSelect;


    #region stats
    [Header("Movement stats")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private int _moves = 5;
    #endregion

    private void Update()
    {

        //Case where player is selected and mouse is hovering
        if (IsSelected() && _moves > 0)
        {
            ShowPath();
        }

        //Case where player is selected and mouse is pressed
        if (Input.GetMouseButtonDown(0))
        {

            if (IsSelected())
            {
                _possMoves = BFS.BFSvisited(_onNode, _moves);
                Move();
            }
            else if (!IsSelected())
            {
                HighlightManager.Instance.ClearMovesMap();
                _possMoves = _emptySet;
            }
        }
    }

    private void Start()
    {
        _onNode = GridManager.Instance.tilesDict[new Vector3Int(0, 0, 0)];
        _pSelect = GetComponent<PlayerSelected>();
    }

    private void ShowPath()
    {
        //Clear existing map
        HighlightManager.Instance.ClearPathMap();

        //Find target
        HexNode target = MouseInput.Instance.GetNodeFromMouse();
        if(target == null || !_possMoves.Contains(target) || !target.isWalkable) { return; }

        //Get the path
        List<HexNode> path = PathFinding.FindPath(_onNode, target);
        if (path == null) { return; }

        //Paint the path
        foreach (HexNode node in path)
        {

           HighlightManager.Instance.PathHighlight(node.gridPos);

        }

    }

    private void Move()
    {
        //Player is selected,  and tile is walkable
        if (MouseInput.Instance.IsTileWalkable() && !_pSelect.cursorInside)
        {
            //What we clicked after selecting player
            HexNode target = GridManager.Instance.tilesDict[MouseInput.Instance.GetCellPosFromMouse()];

            //Check that we have enough moves to make it
            if(_possMoves.Contains(target))
            {
                //Clear the highlighted map
                HighlightManager.Instance.ClearMovesMap();

                //A* find path and then walk it
                List<HexNode> path = PathFinding.FindPath(_onNode, target);
                StartCoroutine(Walk(path));
                _onNode = target;
 
            }
        }
    }

    IEnumerator Walk(List<HexNode> path)
    {

        //Iterate and move tile by tile
        for(int i = path.Count-1; i >=0 ;i--)
        {
            MovePlayerToHex(path[i].transform.position);
            _moves--;
            yield return new WaitForSeconds(.2f);
        }

        //Update the possible moves
        _possMoves = BFS.BFSvisited(path[0], _moves);
    }

    private void MovePlayerToHex(Vector3 position)
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

    private bool IsSelected()
    {
        return SelectionManager.Instance.IsThisSelected(this.gameObject);
    }
}
