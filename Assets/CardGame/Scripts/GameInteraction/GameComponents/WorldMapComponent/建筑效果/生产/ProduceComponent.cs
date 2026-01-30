using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class ProduceComponent:BaseComponent
{
    public ProduceRecipe ProduceRecipe;
    public ProduceComponent(CardModel cardModel, ProduceComponentCreator creator) : base(cardModel, creator)
    {
        ProduceRecipe = new ProduceRecipe();
        foreach (var x in creator.RequiredItemNames)
        {
            ProduceRecipe.RequiredItemNames[x.Key] = new();
            foreach (var y in x.Value)
            {
                ProduceRecipe.RequiredItemNames[x.Key].Add(y.FindCardModel().GetComponent<ProduceComponent>());
            }
        }

        foreach (var x in creator.OutputItemNames)
        {
            ProduceRecipe.OutputItemNames.Add(x);
        }
    }
}

public class ProduceComponentCreator : BaseComponentCreator<ProduceComponent>
{
    public Dictionary<ItemType,List<SpaceCardConfig>> RequiredItemNames = new();
    public List<ItemType> OutputItemNames = new();
    public override ComponentType ComponentName()
    {
        return ComponentType.ProduceComponent;
    }

    public override IComponent Create(CardModel cardModel)
    {
        return new ProduceComponent(cardModel, this);
    }
}
/// <summary>
/// 当前的物品
/// </summary>
public class Item
{
    public ItemType Type;
    public ProduceComponent ProduceComponent;
    /// <summary>
    /// 创建物品
    /// </summary>
    public void CreateCardModel()
    {
        
    }

    public ItemType GetKey()
    {
        return Type;
    }
}
/// <summary>
/// 生产的来源
/// </summary>
public class ProduceRecipe
{
    public Dictionary<ItemType,List<ProduceComponent>> RequiredItemNames = new();
    public List<ItemType> OutputItemNames = new();
}
