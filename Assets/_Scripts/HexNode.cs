using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class HexNode : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] public bool isWalkable;
    private Vector3Int _gridPos;

    #region Pathfinding
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public List<HexNode> Neighbors { get; protected set; }
    public HexNode Connection { get; private set; }

    public void SetConnection(HexNode node)
    {
        Connection = node;
    }

    public void SetG(float g){G = g;}

    public void SetH(float h){H = h;}

    #endregion

    private void Awake()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
    }
    public void Init(Vector3Int pos)
    {
        _gridPos = pos;
    }
    
}



