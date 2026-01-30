using System;
using System.Collections.Generic;
using System.Linq;

public class PlanAction
{
    public string Name;

    // 前置条件（必须全部满足）
    public HashSet<string> Preconditions = new();

    // 效果
    public HashSet<string> AddEffects = new();
    public HashSet<string> RemoveEffects = new();
    public Func<AIBehave> AIBehave;
    public bool IsApplicable(WorldState state)
    {
        return Preconditions.All(state.Has);
    }

    public WorldState Apply(WorldState state)
    {
        var next = state.Clone();
        foreach (var r in RemoveEffects)
            next.Facts.Remove(r);
        foreach (var a in AddEffects)
            next.Facts.Add(a);
        return next;
    }
    public AIBehave GetBehave()
    {
        return AIBehave();
    }
}