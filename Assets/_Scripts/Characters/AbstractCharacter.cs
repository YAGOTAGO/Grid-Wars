using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class AbstractCharacter : NetworkBehaviour
{
    public HashSet<EffectBase> Effects = new();
    public NetworkVariable<int> Health = new();
    public int MaxHealth = 20;
    public NetworkVariable<int> CharacterID { get; private set; } = new(-1);
    public NetworkVariable<Vector3Int> HexGridPosition = new(new Vector3Int(-1,-1,-1));

    private HexNode NodeOn;
    public HexNode GetNodeOn() { return NodeOn; }

    #region Network methods
    public override void OnNetworkSpawn()
    {
        Health.OnValueChanged += HealthChange;
        HexGridPosition.OnValueChanged += UpdateNodeOn;
        SetAllyEnemy();
        AddThisToCharacterDB();
    }

    public override void OnNetworkDespawn()
    {
        Health.OnValueChanged -= HealthChange;
        HexGridPosition.OnValueChanged -= UpdateNodeOn;
        UnsetAllyEnemy();
    }

    #region Caching characters
    private void UnsetAllyEnemy()
    {
        if(IsOwner)
        {
            Database.Instance.Allies.Remove(this);
        }
        else
        {
            Database.Instance.Enemies.Remove(this);
        }
    }

    private void SetAllyEnemy()
    {
        if (IsOwner)
        {
            Database.Instance.Allies.Add(this);
        }
        else
        {
            Database.Instance.Enemies.Add(this);
        }
    }

    private void AddThisToCharacterDB()
    {
        CharacterID.Value =  Database.Instance.AbstractCharactersDB.Add(this);
    }
    #endregion 
    
    private void HealthChange(int prevVal, int newVal)
    {
        OnHealthChanged();
    }

    public void UpdateNodeOn(Vector3Int preVal, Vector3Int newVal)
    {
        NodeOn = GridManager.Instance.GridCoordTiles[newVal];
        
        if(IsOwner)
        {
            NodeOn.OnEnterSurface(this);
        }
    }

    public void SetNodeOn(HexNode node)
    {
        if(IsServer)
        {
            HexGridPosition.Value = node.GridPos.Value;
        }
        else
        {
            SetNodeOnServerRPC(node.GridPos.Value);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetNodeOnServerRPC(Vector3Int nodeGridPos)
    {
        HexGridPosition.Value = nodeGridPos;
    }

    protected void SetHealth(int health)
    {
        if(IsServer)
        {
            Health.Value = health;
        }
        else
        {
            SetHealthServerRPC(health);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetHealthServerRPC(int health)
    {
        Health.Value = health;
    }

    public void Heal(int heal)
    {
        if (IsServer)
        {
            Health.Value += heal;
        }
        else
        {
            HealServerRPC(heal);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void HealServerRPC(int heal)
    {
        Health.Value += heal;
    }

    public void TakeDamage(int damage)
    {
        if (IsServer)
        {
            Health.Value -= damage;
        }
        else
        {
            TakeDamageServerRPC(damage);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TakeDamageServerRPC(int damage)
    {
        Health.Value -= damage;
    }
    #endregion

    #region Abstract/Virtual Methods
    protected virtual void OnHealthChanged() { }
    public abstract void AddEffect(EffectBase ef);
    protected abstract void RemoveEffect(EffectBase ef);

    /// <summary>
    /// Puts the character on given hexnode
    /// </summary>
    /// <param name="target">HexNode we are placing character on</param>
    /// <param name="positionCharacterOnNode">Whether to change the characters position</param>
    public virtual void PutOnHexNode(HexNode target, bool positionCharacterOnNode)
    {
        if(NodeOn != null)
        {
            //Set node we are leaving to be free
            NodeOn.SetSurfaceWalkable(true);
            NodeOn.SetCharacterOnNode(-1); //-1 will set a null character reference
        }
 
        SetNodeOn(target); //yield until this is done
        target.SetSurfaceWalkable(false);
        target.SetCharacterOnNode(CharacterID.Value);
        //target.OnEnterSurface(this);

        if (positionCharacterOnNode)
        {
           transform.position = target.transform.position;
        }

    }
    #endregion

}
