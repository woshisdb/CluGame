using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFinishNode : FinishExeNode
{
    public TestFinishNode() : base("Finish", "deSc")
    {
    }

    public override bool CanClickChange(TaskPanelModel task)
    {
        //throw new System.NotImplementedException();
        task.SetExeNode(new Test_SelectExeNode());
        var cards = GetCards(task);
        foreach (var x in cards)
        {
            GameFrameWork.Instance.AddCardByCardData(x, new Vector3(0, 0, 0));
        }
        return true;
    }

    //public override bool CanProcess(TaskPanelModel task)
    //{
    //    return true;
    //}

    public override List<CardData> GetCards(TaskPanelModel model)
    {
        return new List<CardData>()
        {
            new TestCardData()
        };
    }

    public override ExeEnum GetExeEnum()
    {
        return ExeEnum.e3;
    }

    //public override void Process(TaskPanelModel task)
    //{
    //    task.SetExeNode(new Test_SelectExeNode());
    //}

    public override bool WhenCardChange(TaskPanelModel task)
    {
        return false;
    }
}
