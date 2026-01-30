using System.Collections.Generic;

public interface ISearchNode
{
    bool IsGoal(ISearchNode goal);
    IEnumerable<SearchAction> GetNextNode();
}

public class SearchAction
{
    public int Cost;
    public ISearchNode SearchNode;

    public SearchAction(int Cost,ISearchNode searchNode)
    {
        this.Cost = Cost;
        this.SearchNode = searchNode;
    }
}

public class SearchResult
{
    public ISearchNode GoalNode;
    public List<ISearchNode> Path;
}

public static class SearchAlgorithm
{
    public static SearchResult Search(
        ISearchNode start,
        ISearchNode goal,
        int maxIterations = 1000)
    {
        // open list
        var open = new List<ISearchNode>();

        // 记录路径
        var cameFrom = new Dictionary<ISearchNode, ISearchNode>();

        // 记录到达节点的最小代价
        var costSoFar = new Dictionary<ISearchNode, int>();

        open.Add(start);
        costSoFar[start] = 0;

        int iterations = 0;

        while (open.Count > 0 && iterations++ < maxIterations)
        {
            // 取当前代价最小的节点
            var current = GetLowestCostNode(open, costSoFar);
            open.Remove(current);

            if (current.IsGoal(goal))
            {
                return new SearchResult
                {
                    GoalNode = current,
                    Path = ReconstructPath(cameFrom, current)
                };
            }

            foreach (var action in current.GetNextNode())
            {
                var next = action.SearchNode;
                int newCost = costSoFar[current] + action.Cost;

                if (costSoFar.TryGetValue(next, out int oldCost) &&
                    newCost >= oldCost)
                    continue;

                costSoFar[next] = newCost;
                cameFrom[next] = current;

                if (!open.Contains(next))
                    open.Add(next);
            }
        }

        return null;
    }

    private static ISearchNode GetLowestCostNode(
        List<ISearchNode> nodes,
        Dictionary<ISearchNode, int> costSoFar)
    {
        ISearchNode best = null;
        int bestCost = int.MaxValue;

        foreach (var node in nodes)
        {
            int cost = costSoFar[node];
            if (cost < bestCost)
            {
                bestCost = cost;
                best = node;
            }
        }

        return best;
    }

    private static List<ISearchNode> ReconstructPath(
        Dictionary<ISearchNode, ISearchNode> cameFrom,
        ISearchNode goal)
    {
        var path = new List<ISearchNode>();
        var current = goal;

        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(current); // start
        path.Reverse();

        return path;
    }
}

