using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 玩家的循环
/// </summary>
public class GameLoopDicItem
{
    public string name;
    public UnityAction<Action> befRunFunc;
    public UnityAction<Action> initFunc;
    public UnityAction<Action> runFunc;
    public UnityAction<Action> afterRunFunc;
    public void Init(Action done)
    {
        if (initFunc!=null)
        {
            initFunc?.Invoke(done);
        }
        else
        {
            done?.Invoke();
        }
    }

    public void BefRun(Action done)
    {
        if (befRunFunc!=null)
        {
            befRunFunc?.Invoke(done);
        }
        else
        {
            done?.Invoke();
        }
    }

    public void Run(Action done)
    {
        // Debug.Log("PlayerGameLoop");
        if (runFunc!=null)
        {
            runFunc?.Invoke(done);
        }
        else
        {
            done?.Invoke();
        }
    }

    public void AfterRun(Action done)
    {
        if (afterRunFunc!=null)
        {
            afterRunFunc?.Invoke(done);
        }
        else
        {
            done?.Invoke();
        }
    }
}

public class GameLogicConfig
{
    public GameLogic GameLogic;
    public List<GameLoopDicItem> gameLoop;

    public GameLogicConfig()
    {
        gameLoop = new List<GameLoopDicItem>();
    }
}
