using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchCardRequire : CardRequire
{
    public ResearchCardRequire() : base("研究")
    {
    }

    public override bool Require(CardModel card)
    {
        return card.hasFlag(CardFlag.skill);
    }

}
[Serializable]
public class ResearchSelectExeNode : SelectExeNode
{
    public CardRequire require;
    public ResearchSelectExeNode() : base("研究", "研究事务")
    {
        require = new ResearchCardRequire();
        cardRequires = new List<CardRequire>()
        {
            require
        };
    }
    public override bool CanClickChange(TaskPanelModel task)
    {
        task.SetExeNode(new TestTimeNode());
        return true;
    }
    public override ExeEnum GetExeEnum()
    {
        return ExeEnum.e1;
    }
    public override bool WhenCardChange(TaskPanelModel task)
    {
        if(require.Satify(task))
        {
            task.SetExeNode(new ReadBookSelectExeNode(require));
            return true;
        }
        return false;
    }
}