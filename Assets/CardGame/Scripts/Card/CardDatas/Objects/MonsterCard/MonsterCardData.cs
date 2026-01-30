using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public struct MonsterCardCreateInfo : CardCreateInfo
{
    public MonsterConfig monsterConfig;
    public string monsterId;
    public CardEnum Belong()
    {
        return CardEnum.Monster;
    }
}

/// <summary>
/// 人类或怪物都可继承自这个(玩家也是)
/// </summary>
public class MonsterCardData : NpcCardData,CardDataDic<MonsterEnum,MonsterConfig>
{
    public Dictionary<MonsterEnum, MonsterConfig> MonsterConfigs;
    public MonsterCardData() : base()
    {
        this.Init(ref MonsterConfigs);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.MonsterCard;
    }
    public override CardModel CreateModel(CardCreateInfo CardCreateInfo)
    {
        return CreateCardModelByInfo(CardCreateInfo);
    }
    public override CardModel CreateCardModelByInfo(CardCreateInfo data)
    {
        var info = (MonsterCardCreateInfo)data;
        var monster = new MonsterCardModel(info.monsterId,this,info.monsterConfig);
        return monster;
    }
    [Button]
    public CardModel CreateMonster(MonsterCardCreateInfo data)
    {
        var ret = CreateCardModelByInfo(data);
        GameFrameWork.Instance.data.saveFile.AddNpc((NpcCardModel)ret);
        return ret;
    }
    public string GetResStr()
    {
        return "卡片库/怪物";
    }

    public Dictionary<MonsterEnum, MonsterConfig> GetDic()
    {
        return MonsterConfigs;
    }
}
/// <summary>
/// 怪物卡
/// </summary>
public class MonsterCardModel : NpcCardModel
{
    [SerializeField]
    public override CardData cardData
    {
        get
        {
            return GameFrameWork.Instance.gameConfig.CardMap[CardEnum.MonsterCard];
        }
    }
    public MonsterCardModel(string id,CardData cardData,MonsterConfig cfg) : base(id,cardData,cfg)
    {
    }

    public MonsterConfig Cfg
    {
        get
        {
            return this.cfg.Value as MonsterConfig;
        }
    }
    // public void OnAttackSucc(AttackMapBehave attackMapBehave)
    // {
    //     
    // }
    //
    // public void OnAttackFail(AttackMapBehave attackMapBehave)
    // {
    //     throw new System.NotImplementedException();
    // }

    // public override void OnBeAttackFail(AttackMapBehave attackMapBehave)
    // {
    //     
    // }

    // public override void OnBeAttackSucc(AttackMapBehave attackMapBehave)
    // {
    //     if (attackMapBehave.to.CardModel == this)//是自己的话
    //     {
    //         var behave = attackMapBehave.attack;
    //         var bodyCfg = attackMapBehave.body;
    //         var bodyData = GetBodyDataByCfg(bodyCfg);
    //         int val = bodyData.GetBeAttackVal(behave);
    //         ChangeHpByBody(val,bodyCfg);
    //     }
    // }
    // public override List<MonsterBody> GetBodys()
    // {
    //     var ret = new List<MonsterBody>();
    //     foreach (var x in Cfg.monsterBodies)
    //     {
    //         GetChildBody(x,ref ret);
    //     }
    //     return ret;
    // }

    // public void GetChildBody(MonsterBody body,ref List<MonsterBody> outVals)
    // {
    //     outVals.Add(body);
    //     foreach (var x in body.monsterBodys)
    //     {
    //         GetChildBody(x,ref outVals);
    //     }
    // }
    // [Button("肢体血量模块")]
    // public void InitNpcHPModule()
    // {
    //     Cfg.InitNpc(this);
    // }

    // public override int GetHp()
    // {
    //     var hpModule= this.GetObjectByKey<HPComponent>(MonsterConfig.HpModuleKey);
    //     return hpModule.HP;
    // }

    // public void ChangeHpByBody(int chp,MonsterBody body)
    // {
    //     ChangeHp(chp);
    //     var data = GetBodyDataByCfg(body);
    //     var hasBreak = data.TryAttack(chp);
    // }

    // public override void ChangeHp(int chp)
    // {
    //     var hpModule= this.GetObjectByKey<HPComponent>(MonsterConfig.HpModuleKey);
    //     if (chp > 0)//加血
    //     {
    //         hpModule.AddHp(chp);
    //     }
    //     else
    //     {
    //         hpModule.ReduceHp(-chp);
    //     }
    // }

    // public MonsterBodyData GetBodyDataByCfg(MonsterBody bodyCfg)
    // {
    //     var hpModule= this.GetObjectByKey<HPComponent>(MonsterConfig.HpModuleKey);
    //     return hpModule.bodyDatas[bodyCfg.name];
    // }
}
