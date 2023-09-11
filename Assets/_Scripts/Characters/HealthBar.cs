using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider _healthBarSlider;
    [SerializeField] private TextMeshProUGUI _healthTMP;
    private int _maxHealth = 0;
    private int _health = 0;

    private void Awake()
    {
        _healthBarSlider = GetComponent<Slider>();
        _healthBarSlider.wholeNumbers = true; // Ensure whole numbers only
    }

    private void Start()
    {
        _healthBarSlider.minValue = 0;
        _healthBarSlider.wholeNumbers = true;
    }

    public void InitHealthBarUI(int maxHealth, int health)
    {
        SetMaxHealth(maxHealth);
        SetHealth(health);
    }

    public void SetTMP()
    {
        _healthTMP.text = _health + "/" + _maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        _healthBarSlider.maxValue = maxHealth;
        _maxHealth = maxHealth;
        SetTMP();
    }

    public void SetHealth(int health)
    {
        _healthBarSlider.DOValue(health, 1.5f).SetEase(Ease.OutElastic);
        _health = health;
        SetTMP();
    }
}
