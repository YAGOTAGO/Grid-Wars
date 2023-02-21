using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class HexNode : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] public bool isWalkable;

    [HideInInspector] public Vector3Int _gridPos; //Unity grid x, y, z
    [HideInInspector] public Vector3Int _cubeCoord; //Unity grid converted into cube coords

    #region Pathfinding
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public List<HexNode> Neighboors { get; protected set; }
    public HexNode Connection { get; private set; }

    public void SetConnection(HexNode node)
    {
        Connection = node;
    }

    public void CacheNeighbors()
    {   
        Neighboors = GridManager.Instance.tilesDict.Where(t => HexDistance.GetDistance(this, t.Value) == 1).Select(t => t.Value).ToList();
    
    }

    public void SetG(float g){G = g;}

    public void SetH(float h){H = h;}

    #endregion

    private void Awake()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
    }

    //Inits the Hex
    public void Init(Vector3Int pos)
    {
        _gridPos = pos;
        _cubeCoord = HexDistance.UnityCellToCube(pos);
        _renderer.sprite = _sprites[Random.Range(0, _sprites.Count())];
    }
    
}


