using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviour
{

    public static ActionQueue Instance;
    private readonly Queue<Func<IEnumerator>> methodQueue = new();
    private bool _isQueueRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Used to do coroutines in a list sequentially
    /// </summary>
    /// <param name="method">EnqueueMethod(()=> yourmethodhere(param))</param>
    public void EnqueueMethod(Func<IEnumerator> method)
    {
        methodQueue.Enqueue(method);

        Debug.Log(method.Method.Name + " queued to back");
    
        if (!_isQueueRunning) { StartCoroutine(ProcessQueue()); }
    }

    /// <summary>
    /// Puts the action on the front of the queue
    /// </summary>
    /// <param name="method">EnqueueToFront(()=> yourmethodhere(param))</param>
    public void EnqueueToFront(Func<IEnumerator> method)
    {
        Queue<Func<IEnumerator>> tempQueue = new ();
        tempQueue.Enqueue(method);

        while (methodQueue.Count > 0)
        {
            tempQueue.Enqueue(methodQueue.Dequeue());
        }

        methodQueue.Clear();

        while (tempQueue.Count > 0)
        {
            methodQueue.Enqueue(tempQueue.Dequeue());
        }

        Debug.Log(method.Method.Name + " queued to front");

        if (!_isQueueRunning) { StartCoroutine(ProcessQueue()); }
    }
    
    // Coroutine to process the method queue
    private IEnumerator ProcessQueue()
    {
        _isQueueRunning = true;

        while (methodQueue.Count > 0)
        {
            Func<IEnumerator> method = methodQueue.Dequeue();
            yield return StartCoroutine(method());
        }

        _isQueueRunning = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns true when queue is not running</returns>
    public bool IsQueueStopped()
    {
        return !_isQueueRunning;
    }

}
