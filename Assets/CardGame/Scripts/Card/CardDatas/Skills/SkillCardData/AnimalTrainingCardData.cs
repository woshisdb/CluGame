using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalTrainingCardData : SkillCardData, IStateFlag
{
    public AnimalTrainingCardData() : base()
    {
        title = "驯兽";
        description = "训练动物执行复杂指令，建立长期服从关系，可用于战斗辅助、追踪或传递信息（对野生动物效果有限）。";
        InitCardFlags(typeof(IStateFlag));
    }

    public override CardModel CreateModel()
    {
        return new AnimalTrainingCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.animalTraining;
    }
}



public class AnimalTrainingCardModel : CardModel
{
    public AnimalTrainingCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
}