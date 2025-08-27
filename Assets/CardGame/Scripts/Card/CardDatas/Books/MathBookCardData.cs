//public class MathBookCardData : BookCardData
//{

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MathBookCardData : CardData,IBookFlag
{
    public MathBookCardData() : base()
    {
        title = "��ѧ��";
        description = "��ѧ����UP";
        InitCardFlags(typeof(MathBookCardData));
    }

    public bool CanRead(CardModel card)
    {
        return true;
    }

    public override CardModel CreateModel()
    {
        return new MathBookCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.mathbook;
    }

    public bool Read(CardModel card)
    {
        return true;
    }
}

public class MathBookCardModel : CardModel
{
    public MathBookCardModel(CardData cardData) : base(cardData)
    {

    }
}