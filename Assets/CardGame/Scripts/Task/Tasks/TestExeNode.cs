using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ExeCardRequire : CardRequire
{
    public Test_ExeCardRequire() : base("测试")
    {
    }

    public override bool Require(CardModel card)
    {
        return true;
    }
}
[Serializable]
public class Test_SelectExeNode : SelectExeNode
{
    public Test_SelectExeNode() : base("TITLE", "description")
    {
        cardRequires = new List<CardRequire>()
        {
            new Test_ExeCardRequire(),
            new Test_ExeCardRequire()
        };
    }

    public override bool CanClickChange(TaskPanelModel task)
    {
        task.SetExeNode(new TestTimeNode());
        return true;
    }

    //public override bool CanProcess(TaskPanelModel task)
    //{
    //    return true;
    //}

    public override ExeEnum GetExeEnum()
    {
        return ExeEnum.e1;
    }

    //public override void Process(TaskPanelModel task)
    //{
    //    task.SetExeNode(new TestTimeNode());
    //}

    public override bool WhenCardChange(TaskPanelModel task)
    {
        return false;
        //throw new NotImplementedException();
    }
}
