using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public enum KinshipFactType
{
    [LabelText("父母")]
    Parent,

    [LabelText("配偶")]
    Spouse
}

public enum KinshipType
{
    [LabelText("无血缘关系")]
    None,

    // ================= 直系血亲 =================

    [LabelText("父母")]
    Parent,

    [LabelText("子女")]
    Child,

    // ================= 同辈血亲 =================

    [LabelText("兄弟姐妹")]
    Sibling,

    // ================= 隔代血亲 =================

    [LabelText("祖父母")]
    GrandParent,

    [LabelText("孙子女")]
    GrandChild,

    // ================= 旁系血亲 =================

    [LabelText("叔叔 / 姑姑 / 舅舅 / 阿姨")]
    UncleAunt,

    [LabelText("侄子 / 侄女 / 外甥 / 外甥女")]
    NephewNiece,

    [LabelText("堂兄弟姐妹 / 表兄弟姐妹")]
    Cousin,

    // ================= 婚姻关系 =================

    [LabelText("配偶")]
    Spouse
}

public class KinshipFact
{
    public string FromNpcId;
    public string ToNpcId;
    public KinshipFactType Type;
}

public class KinshipResult
{
    public string TargetNpcId;
    public KinshipType Type;
    public int Degree; // 亲疏度（0 = 直系）
}

public static class WorldKinshipDatabase
{
    private static readonly List<KinshipFact> facts = new();

    // =======================
    // 添加事实（唯一入口）
    // =======================
    
    public static void AddParentChild(string parentId, string childId)
    {
        facts.Add(new KinshipFact
        {
            FromNpcId = parentId,
            ToNpcId = childId,
            Type = KinshipFactType.Parent
        });
    }

    public static void AddSpouse(string a, string b)
    {
        facts.Add(new KinshipFact { FromNpcId = a, ToNpcId = b, Type = KinshipFactType.Spouse });
        facts.Add(new KinshipFact { FromNpcId = b, ToNpcId = a, Type = KinshipFactType.Spouse });
    }

    // =======================
    // 基础查询
    // =======================

    public static List<string> GetParents(string npcId)
    {
        return facts
            .Where(f => f.Type == KinshipFactType.Parent && f.ToNpcId == npcId)
            .Select(f => f.FromNpcId)
            .ToList();
    }

    public static List<string> GetChildren(string npcId)
    {
        return facts
            .Where(f => f.Type == KinshipFactType.Parent && f.FromNpcId == npcId)
            .Select(f => f.ToNpcId)
            .ToList();
    }

    public static List<string> GetSpouses(string npcId)
    {
        return facts
            .Where(f => f.Type == KinshipFactType.Spouse && f.FromNpcId == npcId)
            .Select(f => f.ToNpcId)
            .ToList();
    }

    // =======================
    // 原子判断
    // =======================

    public static bool IsParent(string a, string b)
        => facts.Any(f => f.Type == KinshipFactType.Parent && f.FromNpcId == a && f.ToNpcId == b);

    public static bool IsChild(string a, string b)
        => IsParent(b, a);

    public static bool IsSpouse(string a, string b)
        => facts.Any(f => f.Type == KinshipFactType.Spouse && f.FromNpcId == a && f.ToNpcId == b);

    public static bool IsSibling(string a, string b)
    {
        var pa = GetParents(a);
        var pb = GetParents(b);
        return pa.Intersect(pb).Any();
    }

    // =======================
    // 推导判断
    // =======================

    public static bool IsGrandParent(string a, string b)
    {
        foreach (var parent in GetParents(b))
            if (IsParent(a, parent))
                return true;
        return false;
    }

    public static bool IsUncleAunt(string a, string b)
    {
        foreach (var parent in GetParents(b))
            if (IsSibling(a, parent))
                return true;
        return false;
    }

    public static bool IsCousin(string a, string b)
    {
        foreach (var pa in GetParents(a))
        foreach (var pb in GetParents(b))
            if (IsSibling(pa, pb))
                return true;
        return false;
    }

    // =======================
    // 统一对外接口
    // =======================

    public static KinshipResult GetKinship(string selfId, string targetId)
    {
        if (selfId == targetId)
            return null;

        if (IsSpouse(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.Spouse, Degree = 0 };

        if (IsParent(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.Parent, Degree = 0 };

        if (IsChild(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.Child, Degree = 0 };

        if (IsSibling(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.Sibling, Degree = 1 };

        if (IsGrandParent(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.GrandParent, Degree = 1 };

        if (IsGrandParent(targetId, selfId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.GrandChild, Degree = 1 };

        if (IsUncleAunt(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.UncleAunt, Degree = 2 };

        if (IsUncleAunt(targetId, selfId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.NephewNiece, Degree = 2 };

        if (IsCousin(selfId, targetId))
            return new KinshipResult { TargetNpcId = targetId, Type = KinshipType.Cousin, Degree = 2 };

        return null;
    }
}

public class Kinship
{
    public int TargetNpcId;

    public KinshipType Type;

    /// <summary>
    /// 血缘距离（0=直系，数值越大越远）
    /// </summary>
    public int Degree;

    public override string ToString()
    {
        return $"{Type} (Degree:{Degree})";
    }
}

public class KinshipComponent : BaseComponent
{
    public KinshipComponent(CardModel cardModel, KinshipComponentCreator creator)
        : base(cardModel, creator)
    {
    }

    private string SelfId => CardModel.GetID();

    public KinshipResult GetKinshipWith(string targetNpcId)
    {
        return WorldKinshipDatabase.GetKinship(SelfId, targetNpcId);
    }

    public bool IsKin(string targetNpcId)
    {
        return GetKinshipWith(targetNpcId) != null;
    }
}


public class KinshipComponentCreator
    : BaseComponentCreator<KinshipComponent>
{
    public override ComponentType ComponentName()
    {
        return ComponentType.KinshipComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new KinshipComponent(cardModel, this);
    }
}
