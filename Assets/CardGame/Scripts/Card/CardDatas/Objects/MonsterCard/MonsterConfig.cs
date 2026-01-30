using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

using Sirenix.OdinInspector;
using Unity.Mathematics;

public enum MonsterOrigin
{
    //region 生物实验与基因工程
    [LabelText("基因编辑失控（如实验室改造生物突破限制，产生恶性变异）")]
    GeneticallyEngineeredAbomination,

    [LabelText("生物武器泄露（战争遗留的活体兵器，失去控制后肆虐）")]
    BioWeaponContainmentBreach,

    [LabelText("克隆体缺陷演化（复制过程中基因链断裂，形成非预期形态）")]
    DefectiveCloneMutation,

    [LabelText("跨物种杂交失败（不同生物基因强行融合的畸形产物）")]
    InterspeciesHybridFailure,

    //region 外星文明相关
    [LabelText("外星殖民者/掠夺者（主动入侵的地外生物，适应地球环境后异化）")]
    ExtraterrestrialInvader,

    [LabelText("外星寄生虫/共生体（依附宿主生存，改造宿主形态与意识）")]
    AlienParasite,

    [LabelText("外星文明的“废弃物”（被外星文明遗弃的实验体或工具生物）")]
    AlienDiscard,

    [LabelText("星际瘟疫载体（随陨石/飞船抵达的微生物，感染生物后引发变异）")]
    StellarPlagueVector,

    //region 环境与物理异变
    [LabelText("核辐射诱变（核灾难后，生物暴露于辐射下的恶性演化）")]
    NuclearRadiationMutation,

    [LabelText("化学污染畸变（工业废料/生化毒素导致的生物形态崩坏）")]
    ChemicalPollutionAberration,

    [LabelText("时空异常造物（黑洞/虫洞/维度裂缝中诞生的超物理形态）")]
    SpacetimeAnomalyEntity,

    [LabelText("行星环境适配演化（极端环境下生物为生存演化出的恐怖形态）")]
    ExtremeEnvironmentAdaptation,

    //region 机械与数字异化
    [LabelText("AI失控造物（人工智能制造的杀戮机器，自我迭代后脱离控制）")]
    RogueAICreation,

    [LabelText("机械共生体腐化（人机融合技术失控，机械与血肉恶性共生）")]
    CorruptedMechSymbiont,

    [LabelText("数字意识实体化（网络/数据世界的AI或病毒，突破次元形成物理形态）")]
    DigitalEntityManifestation,

    [LabelText("废弃机械自主演化（长期无人维护的机械，因故障/锈蚀形成的“活体”集群）")]
    FerralMachineEvolution,

    //region 史前与超自然科幻
    [LabelText("史前文明遗种（地球远古文明创造或留存的生物，从休眠中苏醒）")]
    PrehistoricCivilizationRemnant,

    [LabelText("宇宙法则漏洞（物理/数学规律本身的缺陷具象化，非碳基非硅基存在）")]
    CosmicLawAnomaly,

    [LabelText("意识聚合体（大量生物的恐惧/痛苦等情绪具象化形成的能量生物）")]
    CollectiveConsciousnessEntity,

    [LabelText("高维投影（更高维度生物在三维空间的局部显形，形态不可完全解析）")]
    HigherDimensionalProjection
}

public enum MonsterEnum
{
    monster1 = 1
}

/// <summary>
/// 怪物类
/// </summary>
[CreateAssetMenu(fileName = "怪物配置", menuName = "怪物/怪物配置")]
public class MonsterConfig:BaseCardConfig,CardSObj<MonsterEnum>
{
    public static string HpModuleKey = "hpModuleKey";
    public MonsterEnum monsterEnum;
    public string monsterName;
    public MonsterOrigin MonsterOrigin;
    
    /// <summary>
    /// 行动的卡组
    /// </summary>
    public List<MonsterMoveCfg> moveCards;
    /// <summary>
    /// 攻击的卡组
    /// </summary>
    public List<MonsterReproductCfg> reproductCards;
    
    public MonsterConfig():base()
    {
        moveCards = new List<MonsterMoveCfg>();
        reproductCards = new List<MonsterReproductCfg>();
    }

    public MonsterEnum GetEnum()
    {
        return monsterEnum;
    }
}

public struct RelationData
{
    public string relation;
    public string attitude;
}

/// <summary>
/// 对于NPC的性格等一切东西的描述
/// </summary>
public class AINpcMindCfg
{
    /// <summary>
    /// 对npc对描述信息
    /// </summary>
    public string npcInfo;
    /// <summary>
    /// 我在故事中任务目标
    /// </summary>
    public string myAimInStory;
    /// <summary>
    /// 我的过去所做的事情
    /// </summary>
    public string myHistory;
    /// <summary>
    /// 我与其他人关系的描述
    /// </summary>
    public Dictionary<string,RelationData> relationships;
    /// <summary>
    /// 我所擅长的
    /// </summary>
    public string skillDetail;
    /// <summary>
    /// 住所
    /// </summary>
    public string belong;
    /// <summary>
    /// 他的工作
    /// </summary>
    /// <returns></returns>
    public string work;
    /// <summary>
    /// 心理状况
    /// </summary>
    public string mentalState;
    /// <summary>
    /// 性别
    /// </summary>
    public string sex;
}

public class NpcCreateInf
{
    public string name;
    /// <summary>
    /// 对npc的描述信息
    /// </summary>
    public string npcInfo;
    /// <summary>
    /// 性别
    /// </summary>
    public string sex;
    /// <summary>
    /// 自己的目标
    /// </summary>
    public string aim;
    /// <summary>
    /// 过去的重要行为
    /// </summary>
    public string historyBehave;
    /// <summary>
    /// 和各种角色的关系
    /// </summary>
    public Dictionary<string,RelationData> relationships;
    /// <summary>
    /// coc数值能力的描述
    /// </summary>
    public string skillDetail;
    /// <summary>
    /// 住所
    /// </summary>
    public string belong;
    /// <summary>
    /// 他的工作
    /// </summary>
    /// <returns></returns>
    public string work;
    /// <summary>
    /// 心理状态
    /// </summary>
    public string mentalState;
    /// <summary>
    /// 创建配置
    /// </summary>
    /// <returns></returns>
    public MonsterConfig CreateCfg()
    {
        var ret = new MonsterConfig();
        ret.monsterName = name;
        // ret.AddComponentCreator(new HPComponentCreator());
        //-------------------技能参数
        ret.AddComponentCreator(new SkillComponentCreator());//对角色的技能描述
        ret.AddComponentCreator(new BelongComponentCreator());//角色所在的位置
        //-------------------当前所有可以做的事情
        ret.AddComponentCreator(new NeedSleepComponentCreator());//需要睡觉
        ret.AddComponentCreator(new NeedJobComponentCreator());
        // ret.AddComponentCreator(new AttackComponentCreator());
        // ret.AddComponentCreator(new CanBeAttackComponentCreator());
        ret.AddComponentCreator(new EatFoodComponentCreator());
        //-------------------当前的任务配置
        ret.AddComponentCreator(new NowTaskComponentCreator());
        ret.AddComponentCreator(new NowStateComponentCreator());
        ret.AddComponentCreator(new BehavePointComponentCreator());
        //-------------------AI的主观思想
        ret.AddComponentCreator(new AIMindComponentCreator());
        ret.AddComponentCreator(new RelationComponentCreator());
        var aimindCfg = new AINpcMindCfg();
        aimindCfg.npcInfo = npcInfo;
        aimindCfg.myAimInStory = aim;
        aimindCfg.myHistory = historyBehave;
        aimindCfg.belong = belong;
        aimindCfg.work = work;
        aimindCfg.mentalState = mentalState;
        aimindCfg.skillDetail = skillDetail;
        aimindCfg.relationships = relationships;
        aimindCfg.sex = sex;
        var mind = ret.ComponentCreators.Find(e => { return e is AIMindComponentCreator;}) as AIMindComponentCreator;
        mind.AINpcMindCfg = aimindCfg;
        var belongCmp = ret.ComponentCreators.Find(e => { return e is BelongComponentCreator;}) as BelongComponentCreator;
        var space = GameFrameWork.Instance.data.saveFile.Space.Find(e =>
        {
            return e.GetTitle() == belong;
        }) as SpaceCardModel;
        belongCmp.belong.Value = space;
        return ret;
    }
}