using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
/// <summary>
/// 游戏主循环
/// </summary>
public class GameLogic:SerializedMonoBehaviour
{
    public GameLogicConfig GameLogicScriptable;
    [Button("开始一个回合")]
    public Task StartOneState(Action callback)
    {
        var oneCircleComplete = new TaskCompletionSource<bool>();
        AsyncQueue asyncQueue = new AsyncQueue();
        foreach (var item in GameLogicScriptable.gameLoop)
        {
            asyncQueue.Add(done =>
            {
                GameObject.Find("PassStage").GetComponent<PassStageAnim>().ShowAnimation(item.name,() =>
                {
                    item.BefRun(() =>
                    {
                        item.Run(() =>
                        {
                            item.AfterRun(done);
                        });
                    });
                });
            });
            asyncQueue.Add(e =>
            {
                e?.Invoke();
                callback?.Invoke();
                oneCircleComplete.SetResult(true);
            });
        }
        asyncQueue.Run();
        return oneCircleComplete.Task;
    }
    [Button]
    public void InitOneGame()
    {
        foreach (var logic in GameLogicScriptable.gameLoop)
        {
            logic.Init(null);
        }
    }

    public async void Start()
    {
        InitOneGame();
        while (true)
        {
            await StartOneState(null);
        }
    }
}
//startCircle1，循环开始处理
//