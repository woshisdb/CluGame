using System.Collections.Generic;
using System.Text;

/// <summary>
/// ai对需求的检测行为逻辑
/// </summary>
public class NPCNeedAILogic:IAILogic
{
    public NpcCardModel npc;
    public NPCNeedAILogic(NpcCardModel npc)
    {
        this.npc = npc;
    }
    /// <summary>
    /// 没有可执行的任务
    /// </summary>
    /// <returns></returns>
    public AIBehave GetBehaveByState()
    {
        var retData = new Dictionary<string, CardModel>();
        var mind = npc.GetComponent<AIMindComponent>();
        var belong = npc.GetComponent<BelongComponent>().belong.Value;
        if (mind.aimSpace!=null)//如果是有目标位置
        {
            if (mind.aimSpace == belong)
            {
                var ret = TryGetInteractionSpace();
                ret.SetRetData(retData);
                return ret;
            }
            else
            {
                mind.ClearAIMind();
                var ret = GetBehave();
                ret.SetRetData(retData);
                return ret;
            }
        }
        else
        {
            var ret = GetBehave();
            ret.SetRetData(retData);
            return ret;
        }
    }
    public AIBehave GetBehave()
    {
        var getMostImportantNeeds = new AIBehave(e =>
        {
            var needs = npc.GetComponents<INeed>();
            var mostImportantNeedsCard = new List<CardModel>();
            foreach (var need in needs)
            {
                mostImportantNeedsCard.Add(new SelectCard<INeed>(need));
            }

            return mostImportantNeedsCard;
        }, (e) =>
        {
            npc.GetComponent<AIMindComponent>().importantNeed = (e as SelectCard<INeed>).select;
            return ("最需要满足", e);
        }, false, (e) =>
        {
            var sel = e as SelectCard<INeed>;
            return sel.GetSelect().NeedDescription() + ":" + sel.GetSelect().SatLevel();
        }, () =>
        {
            return "选择一个当前最需要满足的一个需求";
        });
        var setMyAimbehave = GetMyAim();
        setMyAimbehave.When(e =>
        {
            return true;
        }, GetLikeSpace());
        getMostImportantNeeds.When(e =>
        {
            return true;
        },setMyAimbehave);
        return getMostImportantNeeds;
    }
    /// <summary>
    /// 当前选择的任务目标
    /// </summary>
    /// <returns></returns>
    protected AIBehave GetMyAim()
    {
        var MyAim = new AIBehave(e =>
        {
            var needCard = e["最需要满足"] as SelectCard<INeed>;
            var need = needCard.select;
            var ret = new List<CardModel>();
            foreach (var val in need.GetWhyNotSat())
            {
                ret.Add(new SelectCard<string>(val));
            }
            return ret;
        }, (e) =>
        {
            var strCard = e as SelectCard<string>;
            npc.GetComponent<AIMindComponent>().myAim = strCard.select;
            return ("目标",e);
        },false, (e) =>
        {
            var strCard = e as SelectCard<string>;
            return strCard.select;
        }, () =>
        {
            return "你当前的目标是什么";
        });
        return MyAim;
    }
    // /// <summary>
    // /// 如何满足当前的目标（最终目标）
    // /// </summary>
    // /// <returns></returns>
    // protected AIBehave GetHowToSatAim()
    // {
    //     var HowToSatAim = new AIBehave(e =>
    //     {
    //         var ret = new List<CardModel>();
    //         foreach (var x in AISequence)
    //         {
    //             ret.Add(new SelectCard<AISustainBehave>(x));
    //         }
    //         return ret;
    //     }, e =>
    //     {
    //         npc.GetComponent<AIMindComponent>().nowBehave = (e as SelectCard<AISustainBehave>).select;
    //         return ("我要执行",e as SelectCard<AISustainBehave>);
    //     },true, e =>
    //     {
    //         var card = e as SelectCard<AISustainBehave>;
    //         return (card.select as AIConclusion).ConclusionAIInfo(npc);
    //     }, () =>
    //     {
    //         var mind = npc.GetComponent<AIMindComponent>();
    //         var ret = new StringBuilder();
    //         ret.Append("当前要满足的需求是：");
    //         ret.Append(mind.importantNeed.NeedDescription());
    //         ret.Append(",");
    //         ret.Append("我的目标是:");
    //         ret.Append(mind.myAim);
    //         ret.Append(",你要选择哪个行为来实现你的目标?");
    //         return ret.ToString();
    //     });
    //     HowToSatAim.When(e =>
    //     {
    //         return true;
    //     },GetLikeSpace());
    //     return HowToSatAim;
    // }
    /// <summary>
    /// 获得想要去的场景
    /// </summary>
    /// <returns></returns>
    public AIBehave GetLikeSpace()
    {
        var ret = new AIBehave(e =>
        {
            var ret = new List<CardModel>();
            foreach (var space in GameFrameWork.Instance.WorldMapSystem.Spaces)
            {
                if (space.IsSatComponent<ConclusionComponent>())
                {
                    ret.Add(new SelectCard<SpaceCardModel>(space));
                }
            }
            return ret;
        }, e =>
        {
            var selCard = e as SelectCard<SpaceCardModel>;
            npc.GetComponent<AIMindComponent>().aimSpace = selCard.select;
            return ("选择场景",selCard.select);
        },false, e =>
        {
            var selCard = e as SelectCard<SpaceCardModel>;
            return selCard.select.ConclusionAIInfo(npc);
        }, () =>
        {
            var mind = npc.GetComponent<AIMindComponent>().myAim;
            return "你的目标是"+mind+"，选择想要去的场景";
        });
        ret.When(e =>
        {
            var card = e as SelectCard<SpaceCardModel>;
            var space = card.select;
            var cfg = space.cfg.Value as SpaceCardConfig;
            if (space == npc.GetComponent<BelongComponent>().belong.Value)
            {
                return true;
            }

            return false;
        },TryGetInteractionSpace());
        ret.When(e =>
        {
            var card = e as SelectCard<SpaceCardModel>;
            var space = card.select;
            var cfg = space.cfg.Value as SpaceCardConfig;
            if (space != npc.GetComponent<BelongComponent>().belong.Value)
            {
                return true;
            }
            return false;
        },GoToAndActionAIBehave());
        return ret;
    }
    /// <summary>
    /// 前往并执行的行为
    /// </summary>
    /// <returns></returns>
    public AIBehave GoToAndActionAIBehave()
    {
        //尝试前往指定地点并执行
        var behave = new AIBehave(null,null,true, null,null);
        behave.SetEndAction(e =>
        {
            npc.GetComponent<AIMindComponent>().nowBehave = new GoToSpaceAIBehave();
            npc.GetComponent<AIMindComponent>().nowBehave.Bind(npc);
        });
        return behave;
    }
    
    /// <summary>
    /// 获取交互的行为，（对场景）
    /// </summary>
    /// <returns></returns>
    public AIBehave TryGetInteractionSpace()
    {
        var space = npc.GetComponent<BelongComponent>().belong.Value as SpaceCardModel;
        var behave = new AIBehave(e =>
        {
            List<CardModel> ret = new();
            ret.Add(new SelectCard<(string, AISustainBehave)>(("这里没有交互可以满足需求选这个",null)));
            foreach (var act in AIInteractionRoot.AISpaceInteractions)
            {
                if (act.IsSat(space,npc))
                {
                    ret.Add(new SelectCard<(string, AISustainBehave)>((act.GetDetailStr(space,npc),act.GetAISustainBehave(space, npc))));
                }
            }
            return ret;
        },e =>
        {
            return ("场景互动",e);
        },true, e =>
        {
            return (e as SelectCard<(string, AISustainBehave)>).select.Item1;
        }, () =>
        {
            var comp = npc.GetComponent<AIMindComponent>();
            return "你现在到目标是"+comp.myAim+"从下面行为中选一项,你想要做些什么？";
        });
        behave.SetEndAction(e =>
        {
            var card = e["场景互动"] as SelectCard<(string, AISustainBehave)>;
            var mind = npc.GetComponent<AIMindComponent>();
            if (card.select.Item2 == null)
            {
                mind.ClearAIMind();
            }
            else
            {
                mind.SetNowBehave(card.select.Item2);
            }
        });
        return behave;
    }
}