using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{

    public static CharacterSelection Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Based on MapLoader num chararacters we add that many prefabs to selection window
        //Debug.Log(MapLoader.Instance.NumOfCharacters.Value);
    }


}
