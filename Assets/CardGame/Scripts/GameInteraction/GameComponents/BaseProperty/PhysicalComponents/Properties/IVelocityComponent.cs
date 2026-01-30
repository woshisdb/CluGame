// 核心速度枚举（按「通用速度等级」划分，适配大部分物体/角色）

using System.Collections.Generic;

public enum SpeedLevel
{
    /// <summary>
    /// 静止（速度=0）
    /// 场景：静止的物体（如宝箱、岩石）、暂停的角色
    /// </summary>
    Stationary = 0,

    /// <summary>
    /// 极慢（0 < 速度 ≤ 1m/s）
    /// 场景：冰块滑动、熔岩流动、受伤缓慢移动的角色
    /// </summary>
    Crawl = 1,

    /// <summary>
    /// 慢速（1 < 速度 ≤ 3m/s）
    /// 场景：角色步行、普通水流、落叶飘落
    /// </summary>
    Slow = 2,

    /// <summary>
    /// 中速（3 < 速度 ≤ 6m/s）
    /// 场景：角色慢跑、马匹步行、箭矢飞行（低速）
    /// </summary>
    Moderate = 3,

    /// <summary>
    /// 快速（6 < 速度 ≤ 10m/s）
    /// 场景：角色奔跑、马匹疾驰、普通攻击动作、火焰喷射
    /// </summary>
    Fast = 4,

    /// <summary>
    /// 极速（10 < 速度 ≤ 20m/s）
    /// 场景：雷电箭飞行、爆炸冲击波、角色冲刺技能
    /// </summary>
    Blazing = 5,

    /// <summary>
    /// 瞬态（速度 > 20m/s，瞬间移动/无持续运动）
    /// 场景：瞬移技能、子弹飞行、激光束
    /// </summary>
    Transient = 6
}

public interface IVelocityComponent:IPhysicalComponent
{
}



public class VelocityComponentCreator:IPhysicalComponentCreator
{
    public ComponentType ComponentName()
    {
        throw new System.NotImplementedException();
    }

    public IComponent Create(CardModel cardModel)
    {
        throw new System.NotImplementedException();
    }
    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}