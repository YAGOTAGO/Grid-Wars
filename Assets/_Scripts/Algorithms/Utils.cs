using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Utils
{
   
    public static void PrintList<T>(List<T> list)
    {
        StringBuilder builder = new();
        foreach (T item in list)
        {
            builder.Append(item.ToString()).Append(" ");
        }

        string result = builder.ToString().Trim();
        Debug.Log(result);
    }

}
