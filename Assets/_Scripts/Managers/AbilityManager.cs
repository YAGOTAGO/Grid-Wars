using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    public IAbility SelectedAbility;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(SelectedAbility == null) { return; }

        List<HexNode> shape = SelectedAbility.GetShape();
        SelectedAbility.Display();

        if(Input.GetMouseButtonDown(0))
        {
            foreach(HexNode node in shape)
            {
                SelectedAbility.DoAbility(node);
            }
        }
        
        SelectedAbility = null;
        ClearDisplay();
    }

    private void ClearDisplay()
    {
        HighlightManager.Instance.ClearMovesMap();
    }
    public HashSet<HexNode> DisplayRange(HexNode pNode, int range)
    {
        HashSet<HexNode> nodesInRange = BFS.BFSvisited(pNode, range);
        return nodesInRange;

    }


}
