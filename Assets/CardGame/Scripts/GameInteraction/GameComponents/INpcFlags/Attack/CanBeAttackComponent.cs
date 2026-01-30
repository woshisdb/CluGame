using System.Collections.Generic;

public class CanBeAttackComponent:IComponent
{
    public List<MonsterBody> monsterBodies;
    public CardModel CardModel;
    public void OnBeAttackSucc(AttackMapBehave attackMapBehave)
    {
        if (attackMapBehave.to.CardModel == CardModel)//是自己的话
        {
            var behave = attackMapBehave.attack;
            var bodyCfg = attackMapBehave.body;
            var bodyData = GetBodyDataByCfg(bodyCfg);
            int val = bodyData.GetBeAttackVal(behave);
            ChangeHpByBody(val,bodyCfg);
        }
    }

    public void OnBeAttackFail(AttackMapBehave attackMapBehave)
    {
        
    }

    public List<MonsterBody> GetBodys()
    {
        var ret = new List<MonsterBody>();
        foreach (var x in monsterBodies)
        {
            GetChildBody(x,ref ret);
        }
        return ret;
    }
    public void GetChildBody(MonsterBody body,ref List<MonsterBody> outVals)
    {
        outVals.Add(body);
        foreach (var x in body.monsterBodys)
        {
            GetChildBody(x,ref outVals);
        }
    }
    public MonsterBodyData GetBodyDataByCfg(MonsterBody bodyCfg)
    {
        var hpModule= CardModel.GetComponent<HPComponent>();
        return hpModule.bodyDatas[bodyCfg.name];
    }
    
    public void ChangeHpByBody(int chp,MonsterBody body)
    {
        ChangeHp(chp);
        var data = GetBodyDataByCfg(body);
        var hasBreak = data.TryAttack(chp);
    }
    
    public void ChangeHp(int chp)
    {
        var hpModule= CardModel.GetComponent<HPComponent>();
        if (chp > 0)//加血
        {
            hpModule.AddHp(chp);
        }
        else
        {
            hpModule.ReduceHp(-chp);
        }
    }
    
    public CanBeAttackComponent(CardModel cardModel,CanBeAttackComponentCreator creator)
    {
        this.CardModel = cardModel;
        monsterBodies =creator.monsterBodies;
    }

    public CardModel GetCard()
    {
        return CardModel;
    }
}


public class CanBeAttackComponentCreator:IComponentCreator
{
    public List<MonsterBody> monsterBodies;

    public CanBeAttackComponentCreator()
    {
        monsterBodies = new();
    }
    public ComponentType ComponentName()
    {
        return ComponentType.CanBeAttackComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new CanBeAttackComponent(cardModel, this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}

