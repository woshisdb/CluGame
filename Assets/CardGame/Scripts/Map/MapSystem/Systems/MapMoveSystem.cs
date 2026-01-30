using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 在地图上行走的管理器
/// </summary>
public class MapMoveSystem:BaseMapSystem
{
    public void Move(RunMapBehave runMapBehave)
    {
        var path = FindPath(runMapBehave.start,runMapBehave.aim);
        if (path != null)
        {
            var npc = runMapBehave.npc;
            runMapBehave.start.Exit(npc);
            runMapBehave.aim.Enter(npc);
        }
    }

    public CellView[,] cellViews
    {
        get
        {
            return mapLoader.cellViews;
        }
    }

    public CellView[,] wallxs
    {
        get
        {
            return mapLoader.wallxs;
        }
    }

    public CellView[,] wallys
    {
        get
        {
            return mapLoader.wallys;
        }
    }
    /// <summary>
    /// 获得相邻的所有节点
    /// </summary>
    /// <summary>
    /// 获得指定位置周围所有可达的相邻节点（排除被墙阻挡的方向）
    /// </summary>
    /// <param name="pos">当前单元格坐标</param>
    /// <returns>可达的相邻CellView列表</returns>
    public List<CellView> GetNeighbor(Vector2Int pos)
    {
        List<CellView> neighbors = new List<CellView>();

        // 检查当前位置是否在有效范围内（防止数组越界）
        if (!IsPosValid(pos))
        {
            Debug.LogWarning($"位置 {pos} 超出网格范围，无法获取邻居");
            return neighbors;
        }

        // 左邻节点：x-1，y不变
        if (!IsWallInDirection(pos, WallDir.left)) // 左侧无墙
        {
            Vector2Int leftPos = new Vector2Int(pos.x - 1, pos.y);
            if (IsPosValid(leftPos)) // 左邻节点在网格内
            {
                neighbors.Add(cellViews[leftPos.x, leftPos.y]);
            }
        }

        // 右邻节点：x+1，y不变
        if (!IsWallInDirection(pos, WallDir.right)) // 右侧无墙
        {
            Vector2Int rightPos = new Vector2Int(pos.x + 1, pos.y);
            if (IsPosValid(rightPos)) // 右邻节点在网格内
            {
                neighbors.Add(cellViews[rightPos.x, rightPos.y]);
            }
        }

        // 上邻节点：x不变，y+1（根据你的坐标系调整，可能是y-1）
        if (!IsWallInDirection(pos, WallDir.up)) // 上方无墙
        {
            Vector2Int upPos = new Vector2Int(pos.x, pos.y + 1);
            if (IsPosValid(upPos)) // 上邻节点在网格内
            {
                neighbors.Add(cellViews[upPos.x, upPos.y]);
            }
        }

        // 下邻节点：x不变，y-1（根据你的坐标系调整，可能是y+1）
        if (!IsWallInDirection(pos, WallDir.down)) // 下方无墙
        {
            Vector2Int downPos = new Vector2Int(pos.x, pos.y - 1);
            if (IsPosValid(downPos)) // 下邻节点在网格内
            {
                neighbors.Add(cellViews[downPos.x, downPos.y]);
            }
        }

        return neighbors;
    }
    
    /// <summary>
    /// 检查指定方向是否有墙（阻挡通行）
    /// </summary>
    /// <param name="pos">当前单元格坐标</param>
    /// <param name="dir">检查的方向</param>
    /// <returns>true=有墙（不可通行）；false=无墙（可通行）</returns>
    private bool IsWallInDirection(Vector2Int pos, WallDir dir)
    {
        // 调用你的GetAroundWall方法获取该方向的墙信息
        CellView wall = GetAroundWall(pos.x, pos.y, dir);

        // 如果墙存在且类型为wall，则视为阻挡
        if (wall != null && wall.CellModel.Value.CellViewType == CellViewType.wall)
        {
            return true;
        }
        return false;
    }
    [Button]
    public CellView GetAroundWall(int x,int y,WallDir wallDir)
    {
        if (wallDir == WallDir.down)
        {
            var gx = wallxs.GetLength(0);
            var gy = wallxs.GetLength(1);
            if (x>=0&&x<gx&&y>=0&&y<gy)
            {
                return wallxs[x, y];
            }
        }

        if (wallDir == WallDir.up)
        {
            y++;
            var gx = wallxs.GetLength(0);
            var gy = wallxs.GetLength(1);
            if (x>=0&&x<gx&&y>=0&&y<gy)
            {
                return wallxs[x, y];
            }
        }

        if (wallDir == WallDir.left)
        {
            var gx = wallys.GetLength(0);
            var gy = wallys.GetLength(1);
            if (x>=0&&x<gx&&y>=0&&y<gy)
            {
                return wallys[x, y];
            }
        }

        if (wallDir == WallDir.right)
        {
            x++;
            var gx = wallys.GetLength(0);
            var gy = wallys.GetLength(1);
            if (x>=0&&x<gx&&y>=0&&y<gy)
            {
                return wallys[x, y];
            }
        }

        return null;
    }
    /// <summary>
    /// 检查坐标是否在网格范围内（防止数组越界）
    /// </summary>
    /// <param name="pos">待检查的坐标</param>
    /// <returns>true=有效；false=无效</returns>
    private bool IsPosValid(Vector2Int pos)
    {
        // 假设cellViews的维度是 [width, height]，且x范围是[0, width-1]，y范围是[0, height-1]
        if (cellViews == null) return false;
        int width = cellViews.GetLength(0);
        int height = cellViews.GetLength(1);
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    public CellView GetPosView(Vector2Int pos)
    {
        return cellViews[pos.x, pos.y];
    }
    
    /// <summary>
    /// 从起点Cell到终点Cell搜索路径
    /// </summary>
    /// <param name="startCell">起点Cell</param>
    /// <param name="endCell">终点Cell</param>
    /// <returns>路径列表（从起点到终点），无路径则返回null</returns>
    public static List<CellView> FindPath(CellView startCell, CellView endCell)
    {
        if (startCell == null || endCell == null)
        {
            Debug.LogError("起点或终点Cell为null");
            return null;
        }

        // 初始化队列（存储待探索的Cell）
        Queue<CellView> queue = new Queue<CellView>();
        // 已访问的Cell（避免重复访问）
        HashSet<CellView> visited = new HashSet<CellView>();
        // 记录前驱Cell（用于回溯路径）
        Dictionary<CellView, CellView> predecessor = new Dictionary<CellView, CellView>();

        // 起点入队
        queue.Enqueue(startCell);
        visited.Add(startCell);

        // BFS主循环
        while (queue.Count > 0)
        {
            CellView currentCell = queue.Dequeue();

            // 找到终点，回溯路径
            if (currentCell == endCell)
            {
                return ReconstructPath(predecessor, startCell, endCell);
            }

            // 获取当前Cell的所有邻居（通过GetNeighbor()方法）
            List<CellView> neighbors = currentCell.GetNeighbor();
            if (neighbors == null) continue; // 防止空引用
            foreach (CellView neighbor in neighbors)
            {
                // 跳过已访问的邻居
                if (visited.Contains(neighbor)) continue;

                // 标记为已访问，记录前驱，并加入队列
                visited.Add(neighbor);
                predecessor[neighbor] = currentCell;
                queue.Enqueue(neighbor);
            }
        }

        // 队列为空仍未找到终点，无路径
        return null;
    }

    /// <summary>
    /// 根据前驱关系回溯路径
    /// </summary>
    private static List<CellView> ReconstructPath(Dictionary<CellView, CellView> predecessor,
                                                CellView startCell, CellView endCell)
    {
        List<CellView> path = new List<CellView>();
        CellView current = endCell;

        // 从终点反向追溯到起点
        while (current != startCell)
        {
            path.Add(current);
            // 如果找不到前驱，说明路径断裂（理论上不会发生）
            if (!predecessor.TryGetValue(current, out CellView prev))
            {
                Debug.LogError("路径回溯失败，前驱关系断裂");
                return null;
            }
            current = prev;
        }

        // 添加起点并反转路径（得到从起点到终点的顺序）
        path.Add(startCell);
        path.Reverse();
        return path;
    }
}
