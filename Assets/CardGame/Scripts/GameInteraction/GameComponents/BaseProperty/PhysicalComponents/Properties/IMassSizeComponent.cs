// 核心质量枚举（按「通用质量等级」划分，适配大部分物体/道具）

using System.Collections.Generic;

// public enum MassGrade
// {
//     /// <summary>
//     /// 无质量（质量 ≈ 0kg）
//     /// 场景：气体、魔法粒子、光线、烟雾、火焰特效
//     /// 特性：不受重力影响、可被微风推动、无碰撞冲击力
//     /// </summary>
//     Massless = 0,
//     /// <summary>
//     /// 极轻（0 < 质量 ≤ 0.1kg）
//     /// 场景：羽毛、纸屑、魔法粉尘、小型花瓣、冰晶碎片
//     /// 特性：受重力影响极小、易被风吹走、拾取无负重、碰撞无伤害
//     /// </summary>
//     Featherweight = 1,
//     /// <summary>
//     /// 轻质（0.1 < 质量 ≤ 1kg）
//     /// 场景：箭矢、硬币、钥匙、小型药剂瓶、匕首柄
//     /// 特性：受重力影响、可被风吹动、单手轻松拾取、碰撞轻微伤害
//     /// </summary>
//     Lightweight = 2,
//     /// <summary>
//     /// 中等质量（1 < 质量 ≤ 10kg）
//     /// 场景：长剑、盾牌、背包、普通宝箱、木桶、小型工具
//     /// 特性：受重力影响明显、需单手/双手拾取、推动较轻松、碰撞中等伤害
//     /// </summary>
//     Mediumweight = 3,
//     /// <summary>
//     /// 重型（10 < 质量 ≤ 50kg）
//     /// 场景：盔甲、大型武器、马匹（幼崽）、中型岩石、铁箱
//     /// 特性：重力影响显著、需双手搬运/合力推动、碰撞高伤害、不易被击飞
//     /// </summary>
//     Heavyweight = 4,
//     /// <summary>
//     /// 超重型（50 < 质量 ≤ 200kg）
//     /// 场景：马车、雕像、大型宝箱、石门、成年马匹
//     /// 特性：极难推动（需技能/工具）、碰撞巨额伤害、完全免疫小型击飞
//     /// </summary>
//     SuperHeavy = 5,
//     /// <summary>
//     /// 巨型质量（质量 > 200kg）
//     /// 场景：城堡城墙、巨型岩石、大树、熔岩池、山脉（部分）
//     /// 特性：不可移动、地形级物体、碰撞可摧毁小型物体、免疫所有击飞
//     /// </summary>
//     ColossalMass = 6
// }
//
// public enum VolumeSize
// {
//     /// <summary>
//     /// 微型（体积 ≤ 0.01m³）
//     /// 场景：宝石、硬币、箭矢、细小碎片、魔法粉尘
//     /// 特性：可堆叠、易被风吹动、不影响碰撞体积
//     /// </summary>
//     Micro = 0,
//
//     /// <summary>
//     /// 小型（0.01m³ < 体积 ≤ 0.1m³）
//     /// 场景：匕首、药剂瓶、钥匙、手雷、小型矿石
//     /// 特性：可单手拾取、堆叠数量有限、轻微碰撞影响
//     /// </summary>
//     Tiny = 1,
//
//     /// <summary>
//     /// 中小型（0.1m³ < 体积 ≤ 1m³）
//     /// 场景：长剑、盾牌、背包、普通宝箱、小型容器
//     /// 特性：单手/双手拾取、不可堆叠、正常碰撞体积
//     /// </summary>
//     Small = 2,
//
//     /// <summary>
//     /// 中型（1m³ < 体积 ≤ 10m³）
//     /// 场景：木桶、桌椅、盔甲、马匹、中型岩石
//     /// 特性：不可拾取（需推动）、占据存储格多、碰撞影响明显
//     /// </summary>
//     Medium = 3,
//
//     /// <summary>
//     /// 大型（10m³ < 体积 ≤ 100m³）
//     /// 场景：马车、雕像、大型宝箱、石门、小型房屋
//     /// 特性：不可推动、阻挡视野/路径、大范围碰撞影响
//     /// </summary>
//     Large = 4,
//
//     /// <summary>
//     /// 巨型（100m³ < 体积 ≤ 1000m³）
//     /// 场景：城堡城墙、巨型岩石、大树、熔岩池（小型）
//     /// 特性：地形级物体、完全阻挡、不可交互（仅环境）
//     /// </summary>
//     Giant = 5,
//
//     /// <summary>
//     /// 超大（体积 > 1000m³）
//     /// 场景：湖泊、山脉、森林、大型熔岩区域
//     /// 特性：全局环境物体、影响区域气候/元素扩散
//     /// </summary>
//     Colossal = 6
// }

public interface IMassSizeComponent:IPhysicalComponent
{
}

public class MassSizeComponent : IMassSizeComponent
{
    public CardModel Card;
    public int MassGrade;
    public int VolumeSize;

    public CardModel GetCard()
    {
        return Card;
    }

    public MassSizeComponent(CardModel card)
    {
        this.Card = card;
    }
}

public class MassSizeComponentCreator:IPhysicalComponentCreator
{
    public int MassGrade;
    public int VolumeSize;
    public ComponentType ComponentName()
    {
        return ComponentType.MassSizeComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        var ret = new MassSizeComponent(cardModel);
        ret.MassGrade = MassGrade;
        ret.VolumeSize = VolumeSize;
        return ret;
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}
