using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [HideInInspector] public static SelectionManager Instance;
    private Character _selectedChar;
    private Character _priorChar;
    private HexNode _selectedNode;
    private HexNode _priorNode;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerSelected();
        }   
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void PlayerSelected()
    {
        _selectedNode = MouseManager.Instance.GetNodeFromMouse();
        if (_selectedNode == null) { return; }

        Character clickedChar = _selectedNode.GetCharacter();
        if (clickedChar == null) { return; }

        if (IsThisSelected(clickedChar) && IsSameNode())
        {
            _selectedChar.OnUnselected();
            _selectedChar = null;
        }
        else if (!IsSameCharacter())
        {
            _selectedChar = clickedChar;
            _selectedChar.OnSelected();
        }
        _priorNode = _selectedNode;
        _priorChar = clickedChar;
    }

    private bool IsSameCharacter()
    {
        if(_priorChar == null) { return false; }
        return _priorChar.Equals(_selectedChar);
    }

    private bool IsSameNode()
    {
        return _selectedNode.Equals(_priorNode);
    }

    public bool IsThisSelected(Character go)
    {
        return go.Equals(_selectedChar);
    }

}
