// 新增：温感类型（描述物体温度对环境/其他物体的影响）

using System.Collections.Generic;

public enum TemperatureType
{
    Frozen,    // 极低温（<-20℃，可冻结液体、造成冻伤）
    Cold,      // 低温（-20℃~0℃，无冻结但有冷感）
    Normal,    // 常温（0℃~30℃，无特殊影响）
    Warm,      // 温热（30℃~60℃，无伤害，可融化少量冰）
    Hot,       // 高温（60℃~300℃，造成烧伤，可快速融化冰/点燃可燃物体）
    Scorching  // 极高温（>300℃，剧烈烧伤，可融化金属、引爆易燃物）
}

/// <summary>
/// 温度特性
/// </summary>
public interface ITemperatureComponent:IPhysicalComponent
{
}

public class TemperatureComponentCreator:IPhysicalComponentCreator
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