using System.Collections.Generic;

public interface INpcLogicActor:IStartPlayerCircle_GameLogic,IEndPlayerCircle_GameLogic
{
    string Id { get; }
}


public class TurnOrderList<T> where T : INpcLogicActor
{
    private LinkedList<T> list = new LinkedList<T>();
    private LinkedListNode<T> currentNode;

    /// 当前行动角色
    public T Current => currentNode != null ? currentNode.Value : default;

    /// 是否为空
    public bool IsEmpty => list.Count == 0;

    #region 初始化

    public void Initialize(IEnumerable<T> actors)
    {
        if (list!=null)
        {
            list.Clear();
        }
        else
        {
            list = new LinkedList<T>();
        }
        foreach (var a in actors)
            list.AddLast(a);

        currentNode = list.First;
    }

    public bool IsLast()
    {
        return currentNode == list.Last;
    }
    public T MoveFirst()
    {
        if (list.Count == 0)
            return default;

        currentNode = list.First;
        return currentNode.Value;
    }
    
    #endregion

    #region 回合推进

    /// 推进到下一个行动者（循环）
    public T MoveNext()
    {
        if (list.Count == 0)
            return default;

        currentNode = currentNode?.Next ?? list.First;
        return currentNode.Value;
    }

    #endregion

    #region 添加

    /// 添加到末尾
    public void AddLast(T actor)
    {
        list.AddLast(actor);
        if (currentNode == null)
            currentNode = list.First;
    }

    /// 添加到指定节点后
    public void AddAfter(T target, T actor)
    {
        var node = FindNode(target);
        if (node == null)
        {
            AddLast(actor);
            return;
        }

        list.AddAfter(node, actor);
    }

    /// 添加到指定 index（0-based）
    public void AddAt(int index, T actor)
    {
        if (index <= 0)
        {
            list.AddFirst(actor);
            if (currentNode == null)
                currentNode = list.First;
            return;
        }

        if (index >= list.Count)
        {
            AddLast(actor);
            return;
        }

        var node = list.First;
        for (int i = 0; i < index - 1; i++)
            node = node.Next;

        list.AddAfter(node, actor);
    }

    #endregion

    #region 删除

    public void Remove(T actor)
    {
        var node = FindNode(actor);
        if (node == null)
            return;

        // 如果删除的是当前行动者
        if (node == currentNode)
        {
            currentNode = node.Next ?? node.Previous ?? null;
        }

        list.Remove(node);

        if (list.Count == 0)
            currentNode = null;
    }

    #endregion

    #region 查找

    private LinkedListNode<T> FindNode(T actor)
    {
        var node = list.First;
        while (node != null)
        {
            if (node.Value.Id == actor.Id)
                return node;
            node = node.Next;
        }
        return null;
    }

    #endregion
}
