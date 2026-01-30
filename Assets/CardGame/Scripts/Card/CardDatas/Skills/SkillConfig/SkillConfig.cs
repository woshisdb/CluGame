using Sirenix.OdinInspector;
using UnityEngine;

public enum NpcSkill
{
    [LabelText("力量")]
    strength = 1,

    [LabelText("体质")]
    constitution = 2,

    [LabelText("体型")]
    size = 3,

    [LabelText("敏捷")]
    dexterity = 4,

    [LabelText("外貌")]
    appearance = 5,

    [LabelText("智力")]
    intelligence = 6,

    [LabelText("意志")]
    power = 7,

    [LabelText("教育")]
    education = 8,

    [LabelText("幸运")]
    luck = 9,

    [LabelText("理智")]
    sanity = 10,

    [LabelText("生命值")]
    health = 11,

    [LabelText("魔法值")]
    magic = 12,

    [LabelText("侦查")]
    spotHidden = 20,

    [LabelText("聆听")]
    listen = 21,

    [LabelText("心理学")]
    psychology = 22,

    [LabelText("神秘学")]
    occult = 23,

    [LabelText("克苏鲁神话")]
    cthulhuMythos = 24,

    [LabelText("考古学")]
    archaeology = 30,

    [LabelText("历史")]
    history = 31,

    [LabelText("信用评级")]
    creditRating = 32,

    [LabelText("急救")]
    firstAid = 40,

    [LabelText("医学")]
    medicine = 41,

    [LabelText("机械维修")]
    mechanicalRepair = 50,

    [LabelText("电气维修")]
    electricalRepair = 51,

    [LabelText("电子学")]
    electronics = 52,

    [LabelText("驾驶")]
    drive = 60,

    [LabelText("闪避")]
    dodge = 61,

    [LabelText("交涉")]
    persuade = 70,

    [LabelText("快速交谈")]
    fastTalk = 71,

    [LabelText("魅惑")]
    charm = 72,

    [LabelText("恐吓")]
    intimidate = 73,

    [LabelText("潜行")]
    stealth = 80,

    [LabelText("锁匠")]
    locksmith = 81,

    [LabelText("追踪")]
    track = 82,

    [LabelText("格斗")]
    brawl = 90,

    [LabelText("射击")]
    firearms = 91,

    [LabelText("投掷")]
    throwing = 92,

    [LabelText("攀爬")]
    climb = 93,

    [LabelText("语言学")]
    linguistics = 100,

    [LabelText("乔装")]
    disguise = 101,

    [LabelText("驯兽")]
    animalTraining = 102,

    [LabelText("表演")]
    performance = 103,

    [LabelText("美术")]
    fineArt = 104,

    [LabelText("天文学")]
    astronomy = 110,

    [LabelText("图书馆使用")]
    libraryUse = 111,

    [LabelText("精神分析")]
    psychoanalysis = 112,
}


[CreateAssetMenu(fileName = "技能配置", menuName = "技能/技能配置")]
public class SkillConfig:SerializedScriptableObject,CardSObj<NpcSkill>
{
    public string title;
    public string descirption;
    public NpcSkill npcSkill;

    public NpcSkill GetEnum()
    {
        return npcSkill;
    }
}