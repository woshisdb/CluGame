using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CardRequire : CardRequire
{
    public Test_CardRequire() : base("测试")
    {
    }

    public override bool Require(CardModel card)
    {
        return true;
    }
}
[Serializable]
public class ResearchSelectExeNode : SelectExeNode
{
    public ResearchSelectExeNode() : base("研究", "研究事务")
    {
        cardRequires = new List<CardRequire>()
        {
            new ResearchCardRequire()
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
        return false;
    }
}