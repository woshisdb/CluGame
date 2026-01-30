using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 倾向于更相信
/// </summary>
public class FriendlyTrustThinkMood:ThinkBaseModule
{
    /// <summary>
    /// 到达多少个 -3 才触发极端负面态度（默认 1）
    /// </summary>
    public int extremeTriggerCount = 1;
    public FriendlyTrustThinkMood(NpcCardModel npc) : base(npc)
    {
    }
    public override int GetThinkType(Dictionary<NpcThinkType, List<int>> things)
    {
        // 没有该类型事件 → 默认中立态度
        if (!things.ContainsKey(GetThinkType()))
            return 0;

        List<int> events = things[GetThinkType()];
        
        int extremeCount = 0;
        foreach (int e in events)
        {
            if (e == -3)
                extremeCount++;
        }

        if (extremeCount >= extremeTriggerCount)
            return -3; // 达到极端触发阈值
        
        float score = 0f;

        foreach (int e in events)
        {
            if (e > 0)
            {
                // 正向事件对友好 NPC 加成更强
                score += e * 1.2f;
            }
            else if (e < 0)
            {
                // 负向事件对友好 NPC 的削弱相对较弱
                score += e * 0.6f;
            }
        }

        // 限制友善内部评分范围（防止极端累积）
        score = Mathf.Clamp(score, -5f, 5f);

        // 将 -5~5 映射到 -3~3
        // score=-5 → -3
        // score=0  → 0
        // score=+5 → +3
        float mapped = Mathf.Lerp(-3f, 3f, (score + 5f) / 10f);

        return Mathf.RoundToInt(mapped);
    }

    public override NpcThinkType GetThinkType()
    {
        return NpcThinkType.Intimacy;
    }
}