using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface AIPlanNode
{
    /// <summary>
    /// 获取AI的行为
    /// </summary>
    /// <param name="npc"></param>
    /// <returns></returns>
    public AISustainBehave GetAIAISustainBehave(NpcCardModel npc);
    /// <summary>
    /// 获取规划行为
    /// </summary>
    /// <returns></returns>
    public PlanAction GetPlanAction(NpcCardModel npc);
}

/// <summary>
/// 一段时间内持续执行的 AI 行为（由多个 AIBehave 组成）
/// </summary>
public abstract class AISustainBehave
{
    public NpcCardModel npc;
    public bool isStart;
    /// <summary>
    /// 获得当前这回合的行为
    /// </summary>
    /// <returns></returns>
    public abstract Task<AIBehave> GetNowBehave();
    /// <summary>
    /// 是否结束
    /// </summary>
    /// <returns></returns>
    public abstract bool IsComplete();
    /// <summary>
    /// 当结束时处理
    /// </summary>
    public abstract void OnComplete();
    /// <summary>
    /// 终端任务
    /// </summary>
    public abstract void OnBreak();
    /// <summary>
    /// 是否可以开始
    /// </summary>
    /// <returns></returns>
    public abstract bool CanStart();
    /// <summary>
    /// 开始时的处理
    /// </summary>
    public abstract Task Start();

    public async Task<bool> Run()
    {
        if (!isStart)
        {
            if (CanStart())
            {
                await Start();
                isStart = true;
            }
            else
            {
                return false;
            }
        }
        var behave =  await GetNowBehave();
        if (behave != null)
        {
            var decision = new AIDecision(behave);
            await decision.Exe(decision.behave);
        }
        if (IsComplete())
        {
            OnComplete();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 中断的原因
    /// </summary>
    /// <param name="reason"></param>
    public virtual void BreakMySelf(string reason)
    {
        npc.GetComponent<AIMindComponent>().BreakBehave(reason);
    }

    public virtual void Bind(NpcCardModel npc)
    {
        this.npc = npc;
    }
}
