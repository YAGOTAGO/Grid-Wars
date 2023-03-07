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
    public HexNode _onNode;
    private HashSet<HexNode> _possMoves = new();
    private readonly HashSet<HexNode> _emptySet = new();
    private HexNode _priorTarget;
    private bool _isWalking = false;
    private Character _thisPlayer;
    #endregion
        
    #region stats
    [Header("Movement stats")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private int _moves = 5;
    #endregion

    void Update()
    {

        if(_isWalking || !IsSelected()) return; //cannot input while walking or not selected

        //Shows path in possible moves
        if (_moves > 0)
        {
            ShowPath();
        }

        //Player is selected and clicks 
        if (Input.GetMouseButtonDown(0))
        {
            Move();
        }
    }
    

    private void Start()
    {
        InitSingletonVars();
        InitComponents();

        _onNode = _gridManager.GridCoordTiles[new Vector3Int(0, 0)];
        _onNode.SetCharacter(_thisPlayer);
        _onNode.IsWalkable = false;

    }        

    private void InitComponents()
    {
        _thisPlayer = GetComponent<Character>();
    }
    private void InitSingletonVars()
    {
        _gridManager = GridManager.Instance;
        _mouseManager = MouseManager.Instance;
        _highlightManager = HighlightManager.Instance;
    }

    public void PlayerSelected()
    {
        _possMoves = BFS.BFSvisited(_onNode, _moves);
    }

    public void PlayerUnselected()
    {
        _highlightManager.ClearMaps();
        _possMoves = _emptySet;
    }

    private void ShowPath()
    {
        
        //Find target
        HexNode target = _mouseManager.GetNodeFromMouse();
        if(target == null || !_possMoves.Contains(target) 
          || !target.IsWalkable || target == _priorTarget) { return; }

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
        //Check whether tile is walkable
        if (_mouseManager.IsTileWalkable())
        {
            //What we clicked after selecting player
            HexNode target = _gridManager.GridCoordTiles[_mouseManager.MouseCellPos];

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
        //set prior node
        _onNode.IsWalkable = true;
        _onNode.SetCharacter(null);

        //Set current node
        _onNode = target;
        _onNode.IsWalkable = false;
        _onNode.SetCharacter(_thisPlayer);
    }

    IEnumerator Walk(List<HexNode> path)
    {
        _isWalking = true;

        //Iterate and move tile by tile
        for(int i = path.Count-1; i >=0 ;i--)
        {
            MovePlayerToHex(path[i].transform.position);
            _moves--;
            yield return new WaitForSeconds(.2f);
        }

        //Update the possible moves
        _possMoves = BFS.BFSvisited(path[0], _moves); // path[0] is destination

        _isWalking = false;
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
