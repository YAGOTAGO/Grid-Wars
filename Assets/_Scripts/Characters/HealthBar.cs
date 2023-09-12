using DG.Tweening;
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
        _healthBarSlider.wholeNumbers = false; // Ensure whole numbers only
        if (health < _health) //lost HP so show red
        {
            _healthBarSlider.fillRect.GetComponent<Image>().color = Color.red;
            _healthBarSlider.DOValue(health, 1f).SetEase(Ease.InFlash).OnComplete(() => HPBaseColor()).SetDelay(.5f);
        }
        else //gained HP
        {
            _healthBarSlider.DOValue(health, 1f).SetEase(Ease.InFlash).SetDelay(.5f).OnComplete(()=> _healthBarSlider.wholeNumbers = true);
        }

        _health = health;
        SetTMP();
    }

    private void HPBaseColor()
    {
        _healthBarSlider.fillRect.GetComponent<Image>().color = Color.green;
        _healthBarSlider.wholeNumbers = true;
    }
}
