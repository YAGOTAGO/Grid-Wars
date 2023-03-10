using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static iTween;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;

    #region Singletons
    private GridManager _gridManager;
    private MouseManager _mouseManager;
    private HighlightManager _highlightManager;
    #endregion

    #region Vars
    private HashSet<HexNode> _possMoves = new();
    private readonly HashSet<HexNode> _emptySet = new();
    private HexNode _priorTarget;
    private Character _player;
    private HexNode _OnNode;
    private bool _canMove = false;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitSingletonVars();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_canMove) { return;}
        SetFunctionVars();
        ShowPath();
        Move();
    }

    private void SetFunctionVars()
    {
        _player = SelectionManager.Instance.SelectedChar;
        _OnNode = _player.GetNodeOn();
    }

    private void InitSingletonVars()
    {
        _gridManager = GridManager.Instance;
        _mouseManager = MouseManager.Instance;
        _highlightManager = HighlightManager.Instance;
    }

    private void ShowPath()
    {
        
        //Dont show path if no moves
        if(_player.Moves <= 0) { return; }

        //Find target
        HexNode target = _mouseManager.GetNodeFromMouse();
        if (target == null || !_possMoves.Contains(target)
          || !target.IsWalkable || target == _priorTarget) { return; }

        //Cache target for next time
        _priorTarget = target;

        //Get the path
        List<HexNode> path = PathFinding.FindPath(_OnNode, target);
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

        if(!Input.GetMouseButtonDown(0)) { return; }

        //Check whether tile is walkable
        if (_mouseManager.IsTileWalkable())
        {
            //What we clicked after selecting player
            HexNode target = _gridManager.GridCoordTiles[_mouseManager.MouseCellPos];

            //Check that we have enough moves to make it
            if (_possMoves.Contains(target))
            {
                //both maps get cleared
                _highlightManager.ClearPathAndMoves();

                //A* find path and then walk it
                List<HexNode> path = PathFinding.FindPath(_OnNode, target);
                StartCoroutine(Walk(path));
                OnNodeSetting(target);
            }
        }
    }

    private void OnNodeSetting(HexNode target)
    {
        //set prior node
        _OnNode.IsWalkable = true;
        _OnNode.SetCharacter(null);

        //Set node vars
        _OnNode = target;
        _OnNode.IsWalkable = false;
        _OnNode.SetCharacter(_player);

        //Set character On Node
        _player.SetNodeOn(target);
    }

    IEnumerator Walk(List<HexNode> path)
    {

        //Iterate and move tile by tile
        for (int i = path.Count - 1; i >= 0; i--)
        {
            MovePlayerToHex(path[i].transform.position);
            _player.Moves--;
            yield return new WaitForSeconds(.2f);
        }

    }

    private void MovePlayerToHex(Vector3 position)
    {

        Hashtable args = new()
        {
            ["speed"] = _player.PlayerSpeed,
            ["easetype"] = _player.EaseType,
            ["islocal"] = true,
            ["position"] = position
        };

        iTween.MoveTo(_player.gameObject, args);

    }

    public void WalkButtonClicked()
    {
        SetFunctionVars();

        _canMove = !_canMove;

        if (_canMove)
        {
            //Gets poss moves and highlights
            _possMoves = BFS.BFSvisited(_OnNode, _player.Moves, true);

            //Cant also cas ability
            AbilityManager.Instance.SetSelectedAbility(null);
        }
        else
        {
            _highlightManager.ClearPathAndMoves();
        }
        

    }

    public void SetCanMoveToFalse()
    {
        if (_canMove)
        {
            HighlightManager.Instance.ClearPathAndMoves();
        }
        _canMove = false;
        
    }

}
