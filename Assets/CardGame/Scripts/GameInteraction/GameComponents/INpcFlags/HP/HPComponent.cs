using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class HPComponent:IComponent
{
    public MonsterCardModel cardModel;
    /// <summary>
    /// 角色的血量
    /// </summary>
    public int HP;
    /// <summary>
    /// 肢体数据
    /// </summary>
    public Dictionary<string, MonsterBodyData> bodyDatas;
    public HPComponent(MonsterCardModel cardModel,HPComponentCreator creator)
    {
        this.cardModel = cardModel;
        HP = creator.initHp;
        var bodys = cardModel.GetComponent<CanBeAttackComponent>().GetBodys();
        bodyDatas = new Dictionary<string, MonsterBodyData>();
        foreach (var x in bodys)
        {
            x.InitBodyHp(cardModel,this);
        }
    }

    public void ReduceHp(int val)
    {
        HP = math.max(HP - val, 0);
    }
    public void AddHp(int val)
    {
        HP = math.max(HP + val, 0);
    }

    public CardModel GetCard()
    {
        return cardModel;
    }
}

public class HPComponentCreator : IComponentCreator
{
    [Header("初始HP")]
    public int initHp;
    public ComponentType ComponentName()
    {
        return ComponentType.HPComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        var monster = (MonsterCardModel)cardModel;
        return new HPComponent(monster,this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return components.Any(e =>
        {
            return e is CanBeAttackComponentCreator;
        });
    }
}