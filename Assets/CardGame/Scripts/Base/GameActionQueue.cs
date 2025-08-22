using System;
using System.Collections.Generic;
using UnityEngine;

public class GameActionQueue
{
    public List<Action<Action>> actions;

    public GameActionQueue()
    {
        actions = new List<Action<Action>>();
    }

    public void Add(Action<Action> action)
    {
        actions.Add(action);
    }
    public void Clear()
    {
        actions.Clear();
    }
    public void Remove(Action<Action> action)
    {
        actions.Remove(action);
    }

    public void Run(Action done)
    {
        Debug.Log("ssss????");
        AsyncQueue queue = new AsyncQueue();
        foreach (Action<Action> action in actions)
        {
            Debug.Log("sss1");
            queue.Add(action);
        }
        queue.Run(done);
    }
}