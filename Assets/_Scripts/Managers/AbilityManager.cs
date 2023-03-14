using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    private AbstractAbility _selectedAbility;

    private HexNode _priorNode;
    private List<HexNode> _shape;
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(_selectedAbility == null) { return; }
        if(SelectionManager.Instance.SelectedChar.Actions<=0 ) { return; }

        //Cant move if in ability
        MovementManager.Instance.SetCanMoveToFalse();

        //Click and have a shape we do ability
        if (Input.GetMouseButtonDown(0) && _shape != null && !EventSystem.current.IsPointerOverGameObject())
        {

            foreach (HexNode node in _shape)
            {
                _selectedAbility.DoAbility(node);
            }
            _selectedAbility = null;
            HighlightManager.Instance.ClearPathAndMoves();
            SelectionManager.Instance.SelectedChar.Actions--;
        }

        HexNode mouseNode = MouseManager.Instance.GetNodeFromMouse();
        if(mouseNode==_priorNode || mouseNode == null) { return; }
        _priorNode = mouseNode;

        //if mouse node not inside range BFS return
        HashSet<HexNode> possRange = BFS.BFSvisited(mouseNode, _selectedAbility.GetRange() , _selectedAbility.ShouldDisplayRange());
        
        //Dont do ability action if mouse is outside the range
        if (!possRange.Contains(mouseNode)) { return; }

        //Get the shape and display it
        _shape = _selectedAbility.GetShape(mouseNode);
        _selectedAbility.Display(_shape);

    }


    public void SetSelectedAbility(AbstractAbility ability)
    {
        if (_selectedAbility == ability)
        {
            _selectedAbility = null;
            HighlightManager.Instance.ClearPathMap();

        }
        else
        {
            _selectedAbility = ability;
        }
        
    }


}
