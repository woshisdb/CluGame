using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : SerializedScriptableObject
{
    public GameObject slotTemplate;
    public Dictionary<ViewType, GameObject> viewDic;
    public Dictionary<ViewType, GameObject> viewDicUI;
    /// <summary>
    /// 任务的配置
    /// </summary>
    [BoxGroup("任务卡")]
    public Dictionary<string, TaskConfig> TaskConfigs;
    /// <summary>
    /// 职位类型的配置
    /// </summary>
    // [BoxGroup("职业卡")]
    // public Dictionary<string,JobCardData> JobCardDatas;
    /// <summary>
    /// Npc的配置
    /// </summary>
    // [BoxGroup("角色卡")]
    // public Dictionary<string, NpcCardData> NpcCardDatas;
    /// <summary>
    /// 一系列怪物的配置
    /// </summary>
    [BoxGroup("怪物配置")]
    public Dictionary<string, MonsterConfig> monsterConfigs;
    [BoxGroup("通用事件卡")]
    public Dictionary<string, List<SearchEventCardData>> SearchEventCard;
    [BoxGroup("开始通用事件卡")]
    public Dictionary<string, List<SearchEventCardData>> StartSearchEventCard;
    [BoxGroup("道路管理")]
    public PathConfig PathConfig;
    public Dictionary<string,IRegisterID> ScriptableDatabase;
    /// <summary>
    /// 物体交互配置
    /// </summary>
    public ObjectTaskConfig objectCfg;
    public GameObject taskTemplate;
    public GameObject kvItemUI;
    public GameObject buttonUI;
    public GameObject tableItemUI;
    public GameObject lineTemplate;
    public SaveData saveData;
    public GameObject GameHandBar;

    public Dictionary<CardEnum, CardData> CardMap = new Dictionary<CardEnum, CardData>();
    [Button]
    public void CardMapInit()
    {
        CardMap = new Dictionary<CardEnum, CardData>()
        {
            {CardEnum.SpaceCard,new SpaceCardData()},
            {CardEnum.skill,new SkillCardData()},
            {CardEnum.cell,new CellData()},
            {CardEnum.npc,new NpcCardData()},
            {CardEnum.MonsterCard,new MonsterCardData()},
            {CardEnum.MonsterAttack,new MonsterAttackCardData()},
            {CardEnum.MonsterBody,new MonsterBodyCardData()},
            { CardEnum.Interaction ,new InteractionCardData()},
            {CardEnum.Monster,new MonsterCardData()},
            { CardEnum.ObjectCard,new ObjectCardData()}
        };
    }
    [Button]
    public CardModel CreateCard(CardCreateInfo cardCreator)
    {
        var obj = CardMap[cardCreator.Belong()].CreateCardModelByInfo(cardCreator);
        CardMap[cardCreator.Belong()].AfterCreate(obj);
        return obj;
    }
    public TaskConfig GetTask(string name)
    {
        TaskConfig res = null;
        TaskConfigs.TryGetValue(name,out res);
        return res;
    }
}
