/// <summary>
/// 可冻结/可融化特性接口（支持固-液形态转换）
/// </summary>
public interface IFreezableComponent:IChemicalComponent
{
    
}

public abstract class FreezableComponentCreator:ChemicalComponentCreator
{
    
}