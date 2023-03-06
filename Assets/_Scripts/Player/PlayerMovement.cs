using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Character))]
public class PlayerMovement : MonoBehaviour
{
    #region Singletons
    private GridManager _gridManager;
    private MouseManager _mouseManager;
    private HighlightManager _highlightManager;
    #endregion

    #region Vars
    private HexNode _onNode;
    private HashSet<HexNode> _possMoves = new();
    private readonly HashSet<HexNode> _emptySet = new();
    private PlayerSelected _pSelect;
    private HexNode _priorTarget;
    private bool isWalking = false;
    private Character _thisPlayer;
    #endregion
        
    #region stats
    [Header("Movement stats")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private int _moves = 5;
    #endregion

    private void Update()
    {

        if(isWalking) return; //cannot input while walking

        //Shows path in possible moves
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
            else
            {
                _highlightManager.ClearMaps();
                _possMoves = _emptySet;
            }
        }
    }
    

    private void Start()
    {
        InitSingletonVars();
        InitComponents();

        _onNode = _gridManager.TilesDict[new Vector3Int(0, 0, 0)];
        _onNode.SetCharacter(_thisPlayer);
        _onNode.isWalkable = false;

        
    }

    private void InitComponents()
    {
        _thisPlayer = GetComponent<Character>();
        _pSelect = GetComponent<PlayerSelected>();
    }
    private void InitSingletonVars()
    {
        _gridManager = GridManager.Instance;
        _mouseManager = MouseManager.Instance;
        _highlightManager = HighlightManager.Instance;
    }

   

    private void ShowPath()
    {
        
        //Find target
        HexNode target = _mouseManager.GetNodeFromMouse();
        if(target == null || !_possMoves.Contains(target) 
          || !target.isWalkable || target == _priorTarget) { return; }

        //Remember target for next time
        _priorTarget = target;

        //Get the path
        List<HexNode> path = PathFinding.FindPath(_onNode, target);
        if (path == null) { return; }

        //Clear existing map
        _highlightManager.ClearPathMap();

        //Paint the path
        foreach (HexNode node in path)
        {
           _highlightManager.PathHighlight(node.GridPos);

        }

    }

    private void Move()
    {
        //Player is selected,  and tile is walkable
        if (_mouseManager.IsTileWalkable() && !_pSelect.CursorInside)
        {
            //What we clicked after selecting player
            HexNode target = _gridManager.TilesDict[_mouseManager.MouseCellPos];

            //Check that we have enough moves to make it
            if(_possMoves.Contains(target))
            {
                //both maps get cleared
                _highlightManager.ClearMaps();    

                //A* find path and then walk it
                List<HexNode> path = PathFinding.FindPath(_onNode, target);
                StartCoroutine(Walk(path));
                OnNodeSetting(target);
            }
        }
    }

    private void OnNodeSetting(HexNode target)
    {
        _onNode.isWalkable = true;
        _onNode.SetCharacter(null);
        _onNode = target;
        _onNode.isWalkable = false;
        _onNode.SetCharacter(_thisPlayer);
    }

    IEnumerator Walk(List<HexNode> path)
    {
        isWalking = true;

        //Iterate and move tile by tile
        for(int i = path.Count-1; i >=0 ;i--)
        {
            MovePlayerToHex(path[i].transform.position);
            _moves--;
            yield return new WaitForSeconds(.2f);
        }

        //Update the possible moves
        _possMoves = BFS.BFSvisited(path[0], _moves); // path[0] is destination

        isWalking = false;
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
        return SelectionManager.Instance.IsThisSelected(_thisPlayer);
    }

    
}
