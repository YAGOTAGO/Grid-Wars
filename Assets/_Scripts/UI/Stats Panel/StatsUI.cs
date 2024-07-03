using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private HorizontalLayoutGroup _effectsUIGroup; //This contains the horizontal layout group UI
    [SerializeField] private HealthBar _healthBar; //info about player healthbar
    [SerializeField] private GameObject _effectUIPrefab;
    [SerializeField] private IconCharacterHighlight _icon;
    private void Awake()
    {
        PlayerPanel.Instance.SetPlayerUI(gameObject);
    }
    public void Initialize(Character character)
    {
        _icon.SetCharacter(character);
        _healthBar.InitHealthBarUI(character.MaxHealth);
    }

    public Transform GetEffectUIGroup()
    {
        return _effectsUIGroup.transform;
    }

    public HealthBar GetHealthBar()
    {
        return _healthBar;
    }

    public GameObject GetEffectPrefab()
    {
        return _effectUIPrefab;
    }
}
