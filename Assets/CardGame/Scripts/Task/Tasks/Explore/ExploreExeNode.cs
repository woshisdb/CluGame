using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreCardRequire : CardRequire
{
    public ExploreCardRequire() : base("探索")
    {
    }

    public override bool Require(CardModel card)
    {
        return card.hasFlag(CardFlag.skill);
    }
}
[Serializable]
public class ExploreSelectExeNode : SelectExeNode
{
    public CardRequire require;
    public ExploreSelectExeNode() : base("探索", "探索这个世界")
    {
        require = new ExploreCardRequire();
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