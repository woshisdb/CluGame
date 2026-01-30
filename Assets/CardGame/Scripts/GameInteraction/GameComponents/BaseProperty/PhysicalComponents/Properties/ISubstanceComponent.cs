// 核心形态枚举（基础物态+游戏专属形态，适配大部分交互场景）

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SubstanceForm
{
    /// <summary>
    /// 固态（Solid）
    /// 核心特性：有固定形状和体积、不可流动、碰撞反馈强
    /// 场景：岩石、金属、木材、冰块、盔甲、宝石
    /// 交互规则：可被推动（依质量）、可碰撞伤害、能被熔化/破碎
    /// </summary>
    [LabelText("固态")]
    Solid = 0,

    /// <summary>
    /// 液态（Liquid）
    /// 核心特性：无固定形状、有固定体积、可流动、易变形
    /// 场景：水、油、熔岩（液态）、毒液、药剂
    /// 交互规则：可填充容器、可流动扩散、能冻结/蒸发、弱碰撞反馈
    /// </summary>
    [LabelText("液态")]
    Liquid = 1,

    /// <summary>
    /// 气态（Gas）
    /// 核心特性：无固定形状和体积、可扩散、穿透性强
    /// 场景：空气、烟雾、毒气、蒸汽、火焰（气态部分）
    /// 交互规则：可弥漫扩散、可被风吹动、能冷凝/燃烧、无碰撞体积
    /// </summary>
    [LabelText("气态")]
    Gas = 2,

    /// <summary>
    /// 等离子态（Plasma）
    /// 核心特性：高温电离、发光发热、穿透性中等、能量集中
    /// 场景：雷电、激光、熔岩核心、电弧、魔法光炮
    /// 交互规则：高伤害、可导电、能引燃物体、碰撞即触发反应
    /// </summary>
    [LabelText("等离子态")]
    Plasma = 3,

    /// <summary>
    /// 晶体态（Crystal）
    /// 核心特性：固态衍生、规则晶格结构、易碎裂、能量传导强
    /// 场景：冰结晶、魔法水晶、宝石、矿石晶体
    /// 交互规则：易碎、导热/导电增强、可储存能量、破碎后可能释放元素
    /// </summary>
    [LabelText("晶体态")]
    Crystal = 4,

    /// <summary>
    /// 粉末态（Powder）
    /// 核心特性：固态细分、无固定形状、可流动、易飘散
    /// 场景：火药、魔法粉尘、沙子、灰烬、雪粉
    /// 交互规则：可被风吹散、可填充容器、遇火可能爆炸/引燃、轻微碰撞反馈
    /// </summary>
    [LabelText("粉末态")]
    Powder = 6,

    /// <summary>
    /// 凝胶态（Gel）
    /// 核心特性：液固混合、高粘性、可变形、不易流动
    /// 场景：史莱姆、胶水、弹性胶体、魔法凝胶
    /// 交互规则：可粘附物体、缓冲碰撞、能被冻结/融化、中等碰撞反馈
    /// </summary>
    [LabelText("凝胶态")]
    Gel = 7
}

public class SubstanceComponent:IPhysicalComponent
{
    public SubstanceForm SubstanceForm;
    public CardModel CardModel;

    public SubstanceComponent(CardModel cardModel,SubstanceComponentCreator creator)
    {
        SubstanceForm = creator.SubstanceForm;
        this.CardModel = cardModel;
    }
    public CardModel GetCard()
    {
        return CardModel;
    }
}

public class SubstanceComponentCreator:IPhysicalComponentCreator
{
    public SubstanceForm SubstanceForm;
    public ComponentType ComponentName()
    {
        return ComponentType.SubstanceComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new SubstanceComponent(cardModel,this);
    }
    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}