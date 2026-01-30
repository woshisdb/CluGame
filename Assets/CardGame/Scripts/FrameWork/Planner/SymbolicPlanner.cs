using System;
using System.Collections.Generic;
using System.Linq;
public static class SymbolicPlanner
{
    private const int MAX_DEPTH = 20; // 防止爆炸

    public static List<PlanAction> Plan(
        WorldState start,
        HashSet<string> goal,
        List<PlanAction> actions)
    {
        var frontier = new Queue<Node>();
        var visited = new HashSet<WorldState>();

        frontier.Enqueue(new Node(start, new List<PlanAction>(), 0));

        while (frontier.Count > 0)
        {
            var node = frontier.Dequeue();
            var current = node.State;

            // ✅ 去重（在这里做，而不是入队时）
            if (visited.Contains(current))
                continue;

            visited.Add(current);

            // ✅ 目标达成
            if (IsGoal(current, goal))
                return node.Plan;

            // ✅ 深度限制
            if (node.Depth >= MAX_DEPTH)
                continue;

            foreach (var action in actions)
            {
                // 前置条件不满足
                if (!action.IsApplicable(current))
                    continue;

                var next = action.Apply(current);

                // ✅ 剪枝 1：Apply 后状态完全没变
                if (next.Equals(current))
                    continue;

                // ✅ 剪枝 2：如果 action 对 goal 完全无关，且不是解锁型行为，可延后
                if (!IsActionRelevant(action, goal) && node.Depth > 5)
                    continue;

                var nextPlan = new List<PlanAction>(node.Plan) { action };
                frontier.Enqueue(new Node(next, nextPlan, node.Depth + 1));
            }
        }

        // 找不到计划
        return null;
    }

    private static bool IsGoal(WorldState state, HashSet<string> goal)
    {
        return goal.All(state.Has);
    }

    /// <summary>
    /// Action 是否对目标“可能有帮助”
    /// </summary>
    private static bool IsActionRelevant(PlanAction action, HashSet<string> goal)
    {
        // 添加了 goal 中的事实
        if (action.AddEffects.Any(goal.Contains))
            return true;

        // 删除了 goal 中的事实（有些规划允许破坏-重建）
        if (action.RemoveEffects.Any(goal.Contains))
            return true;

        return false;
    }

    /// <summary>
    /// 内部搜索节点
    /// </summary>
    private class Node
    {
        public WorldState State;
        public List<PlanAction> Plan;
        public int Depth;

        public Node(WorldState state, List<PlanAction> plan, int depth)
        {
            State = state;
            Plan = plan;
            Depth = depth;
        }
    }
}
