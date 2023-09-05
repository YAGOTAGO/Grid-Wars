using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberedDictionary<T>
{
    private readonly Dictionary<int, T> dictionary = new();
    private int nextKey = 1;
    // Get the count of elements in the NumberedDictionary
    public int Count => dictionary.Count;

    // Add an element and return its assigned number
    public int Add(T item)
    {
        dictionary[nextKey] = item;
        return nextKey++;
    }

    // Retrieve an element by its assigned number
    public T Get(int key)
    {

        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            return default;
        }
    }

    // Check if an element exists with a given number
    public bool Contains(int key)
    {
        return dictionary.ContainsKey(key);
    }

    // Remove an element by its assigned number
    public void Remove(int key)
    {
        dictionary.Remove(key);
    }

}
  