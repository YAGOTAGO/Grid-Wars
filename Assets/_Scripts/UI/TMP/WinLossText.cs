using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WinLossText : MonoBehaviour
{
    private TextMeshProUGUI _winlossTMP;
    private void Awake()
    {
        _winlossTMP = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        _winlossTMP.text = GameManager.Instance.IsWinner ? "YOU WON!" : "UNLUCKY LOSS.";
    }
}
