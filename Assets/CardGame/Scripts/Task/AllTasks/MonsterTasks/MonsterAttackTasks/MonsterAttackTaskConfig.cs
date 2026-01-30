using System;
using System.Collections.Generic;

public static class MonsterAttackTaskConfig
{
    /// <summary>
    /// 创建一个AI可执行的行为
    /// </summary>
    /// <returns></returns>
    public static AIBehave CreateAttackTaskAI(AttackComponent from,CanBeAttackComponent to,Action<Dictionary<string, CardModel>>result)
    {
        Dictionary<string, CardModel> retData = new Dictionary<string, CardModel>();
        var ret = new List<CardModel>();
        foreach (var cfg in from.GetAttackMethods())
        {
            ret.Add(GameFrameWork.Instance.cardsManager.JustCreateCard(new MonsterAttackCardCreateInfo(cfg)));
        }
        var bodRet = new List<CardModel>();
        var bodys = to.GetBodys();
        foreach (var cfg in bodys)
        {
            bodRet.Add(GameFrameWork.Instance.cardsManager.JustCreateCard(new MonsterBodyCardCreateInfo(cfg)));
        }
        var aibehave = new AIBehave(e=>
        {
            return ret;
        }, e =>
        {
            return ("攻击方式",e);
        },false,null,null)
        .SetRetData(retData)
        .SetEndAction(result)
        .When((e) =>
        {
            return e is MonsterAttackCardModel;
        }, new AIBehave(e =>
        {
            return bodRet;
        }, e =>
        {
            return ("肢体", e);
        },true,null,null));
        return aibehave;
    }
    /// <summary>
    /// 所有对怪物的攻击都走这里
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static TaskConfig CreateAttackTask(AttackComponent from,CanBeAttackComponent to,Action<Dictionary<string,CardModel>> result)
    {
        NeedCard_FuncConfigModule selectBodyTask = null;
        var selectTaskWay = new NeedCard_FuncConfigModule("攻击方式","进行攻击",new Dictionary<string, (Func<CardModel, bool>,Func<TaskPanelModel,List<CardModel>>)>()
        {
            {
                "攻击方式", 
                (card =>//卡牌判断条件
                {
                    //攻击卡
                    return card is MonsterAttackCardModel;
                },
                task =>//点击选择的卡牌数目
                {
                    var ret = new List<CardModel>();
                    foreach (var cfg in from.GetAttackMethods())
                    {
                        ret.Add(GameFrameWork.Instance.cardsManager.JustCreateCard(new MonsterAttackCardCreateInfo(cfg)));
                    }
                    return ret;
                })
            },
        });
        selectTaskWay.SetClickChange(task =>
        {
            return task.IsSatCard("攻击方式");
        }, task =>
        {
            // result?.Invoke(task.cardsMap);
            // task.EndTask();
            task.SetExeNode(new TaskConfig(selectBodyTask.title,selectBodyTask));
        });
        selectBodyTask = new NeedCard_FuncConfigModule("攻击肢体","你要攻击那个地方",new Dictionary<string, (Func<CardModel, bool>, Func<TaskPanelModel, List<CardModel>>)>()
        {
            {
                "肢体",
                (card =>
                {
                    return card is MonsterBodyCardModel;
                }, task =>
                {
                    var ret = new List<CardModel>();
                    var bodys = to.GetBodys();
                    foreach (var cfg in bodys)
                    {
                        ret.Add(GameFrameWork.Instance.cardsManager.JustCreateCard(new MonsterBodyCardCreateInfo(cfg)));
                    }
                    return ret;
                })
            }
        });
        selectBodyTask.SetClickChange(task =>
        {
            return task.IsSatCard("肢体");
        }, task =>
        {
            result?.Invoke(task.cardsMap);
            task.EndTask();
        });
        return new TaskConfig(selectTaskWay.title, selectTaskWay);
    }
}