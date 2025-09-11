using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FineArtCardData : CardData, IStateFlag
{
    public FineArtCardData() : base()
    {
        title = "美术";
        description = "创作或鉴赏绘画、雕塑等艺术品，可鉴定艺术品真伪、修复古画，或通过艺术表达传递隐秘信息。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new FineArtCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.fineArt;
    }
}



public class FineArtCardModel : CardModel
{
    public FineArtCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}