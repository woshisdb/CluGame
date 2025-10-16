using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryUseCardData : SkillCardData, IStateFlag
{
    public LibraryUseCardData() : base()
    {
        title = "图书馆使用";
        description = "高效查阅书籍、档案等文献资料，快速定位关键信息，在研究历史、神秘事件或学术问题时非常实用。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new LibraryUseCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.libraryUse;
    }
}



public class LibraryUseCardModel : CardModel
{
    public LibraryUseCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}