using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCardRequire : CardRequire
{
    public TalkCardRequire() : base("交流")
    {
    }

    public override bool Require(CardModel card)
    {
        return card.hasFlag(CardFlag.skill);
    }
}
[Serializable]
public class TalkSelectExeNode : SelectExeNode
{
    public CardRequire require;
    public TalkSelectExeNode() : base("交流", "与人或其他什么交流")
    {
        require = new TalkCardRequire();
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
        if (require.Satify(task))
        {
            task.SetExeNode(new ReadBookSelectExeNode(require));
            return true;
        }
        return false;
    }
}