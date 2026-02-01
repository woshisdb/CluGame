using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;


/// <summary>
/// 一系列的循环逻辑，包含当前区域的所有npc
/// </summary>
public class NpcsCircleLogicModule:SerializedMonoBehaviour,ISendEvent
{
    /// <summary>
    /// 地图管理器
    /// </summary>
    public MapLoader MapLoader;

    public TaskCompletionSource<bool> _playerActionTcs;
    /// <summary>
    /// 一系列的npc行为
    /// </summary>
    public TurnOrderList<NpcCardModel> TurnOrderList;

    public NpcsCircleLogicModule()
    {
        TurnOrderList = new TurnOrderList<NpcCardModel>();
    }
    
    [Button]
    public void StartCicle(Action done)
    {
        var ret = new List<NpcCardModel>();
        var allNpc = GameFrameWork.Instance.playerManager.allNpc;
        foreach (var npc in allNpc)
        {
            if (npc.CanJoin(MapLoader))
            {
                ret.Add(npc);
            }
        }
        TurnOrderList.Initialize(ret);
        done?.Invoke();
    }
    /// <summary>
    /// 玩家操作前的逻辑
    /// </summary>
    /// <param name="npc"></param>
    public void BeforePlayerNpcLogic(NpcCardModel npc)
    {
        foreach (var c in npc.GetComponents<IBeforePlayerNpcLogic>())
        {
            c.OnBeforePlayerNpcLogic();
        }
    }
    /// <summary>
    /// 玩家操作后的逻辑
    /// </summary>
    /// <param name="npc"></param>
    public void AfterPlayerNpcLogic(NpcCardModel npc)
    {
        foreach (var c in npc.GetComponents<IAfterPlayerNpcLogic>())
        {
            c.OnAfterPlayerNpcLogic();
        }
    }
    
    public async Task MainCircle(Action done)
    {
        TurnOrderList.MoveFirst();

        while (TurnOrderList.Current != null)
        {
            BeforePlayerNpcLogic(TurnOrderList.Current);
            await PlayNpcLogic(TurnOrderList.Current);
            AfterPlayerNpcLogic(TurnOrderList.Current);
            if (TurnOrderList.IsLast())
                break;

            TurnOrderList.MoveNext();
        }
        done?.Invoke();
    }
    
    public async void RunAction(Action done)
    {
        await MainCircle(done);
    }
    
    public void BefRunAction(Action done)
    {
        var nowPlayer = GameFrameWork.Instance.playerManager.nowPlayer;
        var belong = nowPlayer.GetComponent<BelongComponent>();
        if (belong.belong.Value!=null)
        {
            var trans = belong.belong.Value.GetTransform();
            GameFrameWork.Instance.CameraGoTo(trans);
        }
        done?.Invoke();
    }
    
    /// <summary>
    /// 前往下一下
    /// </summary>
    public void GoToNext()
    {
        TurnOrderList.MoveNext();
    }

    public void EndLogic()
    {
        
    }

    
    Task WaitPlayerActionFinished(NpcCardModel npc)
    {
        _playerActionTcs = new TaskCompletionSource<bool>();
        npc.BehavePointComponent.SetAfterBehave(PlayerContinueClick);
        return _playerActionTcs.Task;
    }

    public void PlayerContinueClick()
    {
        _playerActionTcs.SetResult(true);
    }
    
    // async Task WaitNPCActionFiinished(NpcCardModel self,List<AICellDecisionSet> cellSets)
    // {
    //     // int index = UnityEngine.Random.Range(0, decisionSets.Count);
    //     // var AINPcDecisionSet = decisionSets[index];
    //     // var decision = AINPcDecisionSet.RandomOne();
    //     // await decision.Exe(decision.behave);
    //     // var cellAIs = new List<AICellSummary>();
    //     // foreach (var cell in cellSets)
    //     // {
    //     //     // var summary = self.NPCAI.ConclusionCell(cell);
    //     //     // cellAIs.Add(summary);
    //     // }
    //     return;
    // }
    async Task WaitNpcWorldSpaceActionFinished(NpcCardModel self)
    {
        var ai = self.GetComponent<AIMindComponent>();
        if (ai.HasTask())
        {
            var isSat = await ai.nowBehave.Run();
            if (isSat)
            {
                ai.ClearAIMind();
            }
        }
        else
        {
            var behave = ai.NPCNeedAILogic.GetBehaveByState();
            var decision = new AIDecision(behave);
            await decision.Exe();
        }
    }
    
    public Task PlayNpcLogic(NpcCardModel npc)
    {
        if (npc.IsPlayer())//npc如果是玩家就走玩家流程
        {
            ShowPlayerUI();
            ///填充行动点
            npc.BehavePointComponent.FillPoint();
            return WaitPlayerActionFinished(npc);
        }
        else//npc如果不为玩家走ai流程
        {
            return Task.CompletedTask;
            HidePlayerUI();
            ///填充行动点
            npc.BehavePointComponent.FillPoint();
            // MapLoader.cellViews
            var belong = npc.GetComponent<BelongComponent>().belong.Value;
            if (belong!=null)
            {
                if (belong is SpaceCardModel)//代表属于的位置是大地图
                {
                    return WaitNpcWorldSpaceActionFinished(npc);
                }
                else//属于私有场景
                {
                    return Task.CompletedTask;
                }
            }
            else
            {
                return Task.CompletedTask;
            }
            
        }
    }
    
    public NpcCardModel nowNpc
    {
        get
        {
            return TurnOrderList.Current;
        }
    }

    public void ShowPlayerUI()
    {
        GameFrameWork.Instance.playerManager.SetPlayerCanOperation(true);
        GameFrameWork.Instance.UIManager.ShowPlayerUI();
    }

    public void HidePlayerUI()
    {
        GameFrameWork.Instance.playerManager.SetPlayerCanOperation(false);
        GameFrameWork.Instance.UIManager.HidePlayerUI();
    }
}