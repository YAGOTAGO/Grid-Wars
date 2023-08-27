using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardKeywords
{

    public static readonly Dictionary<Keyword, string> KeywordDescriptions = new()
    {
        {Keyword.Dash, "<b>Dash:</b> Move up to X in one direction." },

    };

    /// <summary>
    /// Takes a card and boldens keyword in description
    /// </summary>
    /// <param name="card">The card we want description bolden</param>
    public static void BoldenKeywords(CardBase card)
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