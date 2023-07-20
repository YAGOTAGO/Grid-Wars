using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private Card card;

    [SerializeField] private TextMeshProUGUI nameTMP;

    void Start()
    {
        nameTMP.text = card.name;
    }


}
