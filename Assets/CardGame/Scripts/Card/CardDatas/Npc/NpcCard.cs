using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCard : CardData
{
    /// <summary>
    /// NPC的名字
    /// </summary>
    public string NpcName;
    public NpcCard():base()
    {
        viewType = ViewType.NpcCard;
    }
    public override CardModel CreateModel()
    {
        return new NpcCardModel(this);
    }

    public override CardEnum GetCardType()
    {
        return CardEnum.npc;
    }
}
/// <summary>
/// 角色卡
/// </summary>
public class NpcCardModel : CardModel
{
    public NpcCardModel(CardData cardData) : base(cardData)
    {
        // 模型初始化逻辑
    }
    public void InitCardByFlag()
    {
        SetDataByKey(CardEnum.strength.ToString(), 10);
        SetDataByKey(CardEnum.constitution.ToString(), 10);
        SetDataByKey(CardEnum.size.ToString(), 10);
        SetDataByKey(CardEnum.dexterity.ToString(), 10);
        SetDataByKey(CardEnum.appearance.ToString(), 10);
        SetDataByKey(CardEnum.intelligence.ToString(), 10);
        SetDataByKey(CardEnum.power.ToString(), 10);
        SetDataByKey(CardEnum.education.ToString(), 10);
        SetDataByKey(CardEnum.luck.ToString(), 10);
        SetDataByKey(CardEnum.sanity.ToString(), 10);
        SetDataByKey(CardEnum.health.ToString(), 10);
        SetDataByKey(CardEnum.spotHidden.ToString(), 10);
        SetDataByKey(CardEnum.listen.ToString(), 10);
        SetDataByKey(CardEnum.psychology.ToString(), 10);
        SetDataByKey(CardEnum.occult.ToString(), 10);
        SetDataByKey(CardEnum.cthulhuMythos.ToString(), 10);
        SetDataByKey(CardEnum.archaeology.ToString(), 10);
        SetDataByKey(CardEnum.history.ToString(), 10);
        SetDataByKey(CardEnum.creditRating.ToString(), 10);
        SetDataByKey(CardEnum.firstAid.ToString(), 10);
        SetDataByKey(CardEnum.medicine.ToString(), 10);
        SetDataByKey(CardEnum.mechanicalRepair.ToString(), 10);
        SetDataByKey(CardEnum.electricalRepair.ToString(), 10);
        SetDataByKey(CardEnum.electronics.ToString(), 10);
        SetDataByKey(CardEnum.drive.ToString(), 10);
        SetDataByKey(CardEnum.dodge.ToString(), 10);
        SetDataByKey(CardEnum.persuade.ToString(), 10);
        SetDataByKey(CardEnum.stealth.ToString(), 10);
        SetDataByKey(CardEnum.brawl.ToString(), 10);
        SetDataByKey(CardEnum.firearms.ToString(), 10);
        SetDataByKey(CardEnum.fastTalk.ToString(), 10);
        SetDataByKey(CardEnum.locksmith.ToString(), 10);
        SetDataByKey(CardEnum.linguistics.ToString(), 10);
        SetDataByKey(CardEnum.disguise.ToString(), 10);
        SetDataByKey(CardEnum.animalTraining.ToString(), 10);
        SetDataByKey(CardEnum.performance.ToString(), 10);
        SetDataByKey(CardEnum.astronomy.ToString(), 10);
        SetDataByKey(CardEnum.charm.ToString(), 10);
        SetDataByKey(CardEnum.climb.ToString(), 10);
        SetDataByKey(CardEnum.fineArt.ToString(), 10);
        SetDataByKey(CardEnum.intimidate.ToString(), 10);
        SetDataByKey(CardEnum.libraryUse.ToString(), 10);
        SetDataByKey(CardEnum.psychoanalysis.ToString(), 10);
        SetDataByKey(CardEnum.track.ToString(), 10);
        SetDataByKey(CardEnum.throwing.ToString(), 10);
    }
}