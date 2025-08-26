using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCardRequire : CardRequire
{
    public BookCardRequire() : base("测试")
    {
    }
    public override bool Require(CardModel card)
    {
        return card.hasFlag(CardFlag.book)//是否是书籍
    }
}
[Serializable]
public class ReadBookSelectExeNode : SelectExeNode
{
    private CardRequire SkillRequire;
    private CardRequire BookRequire;
    /// <summary>
    /// 需要穿入技能
    /// </summary>
    /// <param name="???"></param>
    public ReadBookSelectExeNode(CardRequire SkillRequire) : base("阅读", "哈哈哈哈哈")
    {
        this.SkillRequire = SkillRequire;
        this.BookRequire = new BookCardRequire();
        cardRequires = new List<CardRequire>()
        {
            SkillRequire，//技能卡
            this.BookRequire//书籍卡
        };
    }

    public override bool CanClickChange(TaskPanelModel task)
    {
        if (SkillRequire.Satify(task)&&this.BookRequire.Satify(task))
        {
            task.SetExeNode(new TestTimeNode());
            return true;
        }
        return false;
    }

    public override ExeEnum GetExeEnum()
    {
        return ExeEnum.e1;
    }

    public override bool WhenCardChange(TaskPanelModel task)
    {
        if (task.TryFindCard(SkillRequire)==null)//没有技能卡就改变
        {
            task.TryReleaseCard(BookRequire);//释放书籍卡
            task.SetExeNode(new ResearchExeNode());//转为研究任务
            return true;
        }
        else//还有技能卡就不变
        {
            return false;
        }
    }
}