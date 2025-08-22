using System;
using System.Collections.Generic;

public class AsyncQueue
{
    private Queue<Action<Action>> taskQueue = new Queue<Action<Action>>();
    private bool isRunning = false;

    public void Add(Action<Action> task)
    {
        taskQueue.Enqueue(task);
    }

    public void Run(Action onComplete = null)
    {
        if (isRunning) return;
        isRunning = true;
        RunNext(onComplete);
    }

    private void RunNext(Action onComplete)
    {
        if (taskQueue.Count == 0)
        {
            isRunning = false;
            onComplete?.Invoke();
            return;
        }

        var task = taskQueue.Dequeue();
        task(() =>
        {
            RunNext(onComplete);
        });
    }
}
