using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardKeywords
{

    public static readonly Dictionary<Keyword, string> KeywordDescriptions = new()
    {
        {Keyword.DASH, "<b>Dash:</b> Move up to X in a straightline." },
        {Keyword.PUSH, "<b>Push:</b> Target gets moved X hexes in a straightline away from the source. Will only push if inline with source." },
        {Keyword.PULL, "<b>Pull:</b> Target gets moved X hexes in a straightline towards the source. Will only pull if inline with source." }
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
    DASH,
    PUSH,
    PULL
}