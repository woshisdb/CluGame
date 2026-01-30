using System.Collections.Generic;

/// <summary>
/// 基础特性，例如基础的化学属性
/// </summary>
public interface IChemicalComponent:IComponent
{
}

public abstract class ChemicalComponentCreator:IComponentCreator
{
    public abstract ComponentType ComponentName();
    public abstract IComponent Create(CardModel cardModel);
    public virtual bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}