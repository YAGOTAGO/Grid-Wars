using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    private AbstractAbility _selectedAbility;

    private HexNode _priorNode;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(_selectedAbility == null) { return; }
            
        HexNode mouseNode = MouseManager.Instance.GetNodeFromMouse();
        if(mouseNode==_priorNode || mouseNode == null) { return; }
        _priorNode = mouseNode;

        //if mouse node not inside range BFS return
        HashSet<HexNode> possRange = BFS.BFSvisited(mouseNode, _selectedAbility.GetRange() , false);
        
        //Dont do ability action if mouse is outside the range
        if (!possRange.Contains(mouseNode)) { return; }

        //Get the shape and display it
        List<HexNode> shape = _selectedAbility.GetShape(mouseNode);
        _selectedAbility.Display(shape);

        if(Input.GetMouseButtonDown(0))
        {
            foreach(HexNode node in shape)
            {
                _selectedAbility.DoAbility(node);
            }
            _selectedAbility = null;
            HighlightManager.Instance.ClearPathAndMoves();
        }
        
    }


    public void SetSelectedAbility(AbstractAbility ability)
    {
        _selectedAbility = ability;
    }


}
