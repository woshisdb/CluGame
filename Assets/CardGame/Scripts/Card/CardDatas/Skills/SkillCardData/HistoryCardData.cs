using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryCardData : CardData, IStateFlag
{
    public HistoryCardData() : base()
    {
        title = "历史";
        description = "回忆特定地区、时期的历史事件、人物和文化背景，帮助辨识古代工具、技术或历史遗留物的意义。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new HistoryCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.history;
    }
}



public class HistoryCardModel : CardModel
{
    public HistoryCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}