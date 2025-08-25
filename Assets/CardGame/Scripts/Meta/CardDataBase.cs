using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase
{
    public static Dictionary<CardEnum, CardData> cardDatabase = new Dictionary<CardEnum, CardData>() {
        { CardEnum.money,new TestCardData() }
    };
    public static CardData GetCard(CardEnum card)
    {
        return cardDatabase[card];
    }
}
