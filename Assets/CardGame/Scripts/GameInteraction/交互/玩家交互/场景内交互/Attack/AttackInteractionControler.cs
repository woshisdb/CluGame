using System.Collections.Generic;

public class AttackInteractionControler:GetInteractionControler
{
    public string GetKey()
    {
        return "攻击";
    }

    public List<UIItemBinder> GetUI(MapLoader map, CardModel beObj, CardModel me)
    {
        var from = beObj.GetComponent<AttackComponent>();
        var to = me.GetComponent<CanBeAttackComponent>();
        var ret = new List<UIItemBinder>();
        ret.Add(new ButtonBinder(() =>
        {
            return GetKey();
        }, () =>
        {
            //任务
            GameFrameWork.Instance.taskManager.StartTask(MonsterAttackTaskConfig.CreateAttackTask(from,to, e =>
            {
                var monster = e["攻击方式"] as MonsterAttackCardModel;
                var body = e["肢体"] as MonsterBodyCardModel;
                var del = new AttackMapBehave(map,from,to,monster.cfg,body.monsterBody);
                del.Run();
                AfterProcess(map,beObj,me);
            }));
        }));
        return ret;
    }

    // public List<AIBehave> GetDecision(MapLoader map, CardModel beObj, CardModel me)
    // {
    //     var ret = new List<AIBehave>();
    //     var from = beObj.GetComponent<AttackComponent>();
    //     var to = me.GetComponent<CanBeAttackComponent>();
    //     var decision = MonsterAttackTaskConfig.CreateAttackTaskAI(from, to, e =>
    //     {
    //         var monster = e["攻击方式"] as MonsterAttackCardModel;
    //         var body = e["肢体"] as MonsterBodyCardModel;
    //         var del = new AttackMapBehave(map, from, to, monster.cfg, body.monsterBody);
    //         del.Run();
    //         AfterProcess(map, beObj, me);
    //     });
    //     ret.Add(decision);
    //     return ret;
    // }


    public bool IsSat(CardModel beObj,CardModel me)
    {
        return me is CanBeAttackComponent && beObj is AttackComponent;
    }

    public void AfterProcess(MapLoader map, CardModel beObj, CardModel me)
    {
        if (beObj is NpcCardModel)
        {
            var obj = beObj as NpcCardModel;
            obj.BehavePointComponent.ReducePoint(1);
        }
    }

    public InteractionType GetInteractionType()
    {
        return InteractionType.Attack;
    }
}