using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardKeywords : MonoBehaviour
{

    private readonly Dictionary<Keyword, string> _keywordDescriptions = new()
    {
        {Keyword.Dash, "Move up to X in a straightline." },

    };


    /// <summary>
    /// Takes a card and boldens keyword in description
    /// </summary>
    /// <param name="card">The card we want description bolden</param>
    public void BoldenKeywords(CardBase card)
    {
        foreach(Keyword keyword in card.Keywords)
        {
            card.Description = card.Description.Replace(keyword.ToString(), $"<b>{keyword}</b>");
        }
    }

}

public enum Keyword
{
    Dash,

}