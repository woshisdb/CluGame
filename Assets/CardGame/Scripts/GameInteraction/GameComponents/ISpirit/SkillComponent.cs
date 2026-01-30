using System.Collections.Generic;

/// <summary>
/// 动物的精神
/// </summary>
public class SkillComponent:IComponent
{
    public static HashSet<NpcSkill> SkillStats = new()
    {
        NpcSkill.strength,
        NpcSkill.constitution,
        NpcSkill.size,
        NpcSkill.dexterity,
        NpcSkill.appearance,
        NpcSkill.intelligence,
        NpcSkill.power,
        NpcSkill.education,
        NpcSkill.luck,
        NpcSkill.sanity,
        NpcSkill.health,
        NpcSkill.spotHidden,
        NpcSkill.listen,
        NpcSkill.psychology,
        NpcSkill.occult,
        NpcSkill.cthulhuMythos,
        NpcSkill.archaeology,
        NpcSkill.history,
        NpcSkill.creditRating,
        NpcSkill.firstAid,
        NpcSkill.medicine,
        NpcSkill.mechanicalRepair,
        NpcSkill.electricalRepair,
        NpcSkill.electronics,
        NpcSkill.drive,
        NpcSkill.dodge,
        NpcSkill.persuade,
        NpcSkill.stealth,
        NpcSkill.brawl,
        NpcSkill.firearms,
        NpcSkill.fastTalk,
        NpcSkill.locksmith,
        NpcSkill.linguistics,
        NpcSkill.disguise,
        NpcSkill.animalTraining,
        NpcSkill.performance,
        NpcSkill.astronomy,
        NpcSkill.charm,
        NpcSkill.climb,
        NpcSkill.fineArt,
        NpcSkill.intimidate,
        NpcSkill.libraryUse,
        NpcSkill.psychoanalysis,
        NpcSkill.track,
        NpcSkill.throwing
    };
    public CardModel CardModel;
    public CardModel GetCard()
    {
        return CardModel;
    }
    public Dictionary<NpcSkill, StateItem<NpcSkill>> dataMap;
    public SkillComponent(CardModel cardModel,SkillComponentCreator creator)
    {
        this.CardModel = cardModel;
        dataMap = new();
        foreach (var s in SkillStats)
        {
            dataMap[s] = new StateItem<NpcSkill>(s,50,100);
        }
    }

    public int GetNowSkill(NpcSkill skill)
    {
        return dataMap[skill].value;
    }

    public void SetNowSkill(NpcSkill skill, int val)
    {
        dataMap[skill].value = val;
    }
}

public class SkillComponentCreator : IComponentCreator
{
    public ComponentType ComponentName()
    {
        return ComponentType.SkillComponent;
    }

    public IComponent Create(CardModel cardModel)
    {
        return new SkillComponent(cardModel,this);
    }

    public bool NeedComponent(List<IComponentCreator> components)
    {
        return true;
    }
}