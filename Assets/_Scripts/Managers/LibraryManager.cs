using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    //used to show the different content windows
    [SerializeField] private GameObject _cardDisplay;
    [SerializeField] private GameObject _equipmentDisplay;
    [SerializeField] private GameObject _burstDisplay;

    //used to add things to content
    [SerializeField] private GridLayoutGroup _cardContent;
    [SerializeField] private GridLayoutGroup _equipmentContent;
    [SerializeField] private GridLayoutGroup _burstContent;

    [SerializeField] private GameObject _displayPrefab;
}
