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
public class Test_SelectExeNode : SelectExeNode
{
    public Test_SelectExeNode() : base("TITLE", "description")
    {
        cardRequires = new List<CardRequire>()
        {
            new Test_CardRequire()
        };
    }

    public override bool CanProcess(TaskPanelModel task)
    {
        throw new System.NotImplementedException();
    }

    public override ExeEnum GetExeEnum()
    {
        return ExeEnum.e1;
    }

    public override void Process(TaskPanelModel task)
    {
        throw new System.NotImplementedException();
    }
}
