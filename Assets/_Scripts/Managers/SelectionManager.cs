using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [HideInInspector] public static SelectionManager Instance;
    public Character SelectedChar { get; private set; }
    private Character _priorChar;
    private HexNode _selectedNode;
    private HexNode _priorNode;

    void Update()
    {
        PlayerClicked();
    }

    void Awake()
    {
        Instance = this;
        
    }

    /// <summary>
    /// A method for handling the clicking over character selection
    /// </summary>
    private void PlayerClicked()
    {
        //Need to get input to go further
        if (!Input.GetMouseButtonDown(0)) { return; }

            _selectedNode = MouseManager.Instance.GetNodeFromMouse();
        if (_selectedNode == null) { return; }

        Character clickedChar = _selectedNode.GetCharacter();
        if (clickedChar == null) { return; }

        //Same node and same character we unselect
        if (IsThisSelected(clickedChar) && IsSameNode())
        {
            SelectedChar.OnUnselected();
            SelectedChar = null;
        }
        //Not the same character then we select that character
        else if (!IsSameCharacter())
        {
            SelectedChar = clickedChar;
            SelectedChar.OnSelected();
        }
        _priorNode = _selectedNode;
        _priorChar = clickedChar;
    }

    private bool IsSameCharacter()
    {
        if(_priorChar == null) { return false; }
        return _priorChar == SelectedChar;
    }

    private bool IsSameNode()
    {
        return _selectedNode == _priorNode;
    }

    public bool IsThisSelected(Character go)
    {
        return go == SelectedChar;
    }

}
