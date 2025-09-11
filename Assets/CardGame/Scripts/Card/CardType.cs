using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡片的符号
/// </summary>
public enum CardFlag
{
    person,///人类卡牌
    Animal,///动物类型
    book,//书籍
    food,
    goods,
    ill,
    knowledge,
    message,
    money,
    placement,
    skill,
    state,
    scene,
    space,
}
/// <summary>
/// 卡牌的枚举类，定义CoC跑团中各类卡牌类型
/// </summary>
public enum CardEnum
{
    /// <summary>
    /// 当前拥有的金币/资金
    /// </summary>
    money,

    /// <summary>
    /// 数学相关的书籍/知识
    /// </summary>
    mathbook,

    /// <summary>
    /// 力量属性相关卡牌
    /// </summary>
    strength,

    /// <summary>
    /// 体质属性相关卡牌
    /// </summary>
    constitution,

    /// <summary>
    /// 体型属性相关卡牌
    /// </summary>
    size,

    /// <summary>
    /// 敏捷属性相关卡牌
    /// </summary>
    dexterity,

    /// <summary>
    /// 外貌属性相关卡牌
    /// </summary>
    appearance,

    /// <summary>
    /// 智力属性相关卡牌
    /// </summary>
    intelligence,

    /// <summary>
    /// 意志属性相关卡牌
    /// </summary>
    power,

    /// <summary>
    /// 教育属性相关卡牌
    /// </summary>
    education,

    /// <summary>
    /// 幸运属性相关卡牌
    /// </summary>
    luck,

    /// <summary>
    /// 理智值相关卡牌
    /// </summary>
    sanity,

    /// <summary>
    /// 生命值相关卡牌
    /// </summary>
    health,

    /// <summary>
    /// 魔法值相关卡牌
    /// </summary>
    magic,
    /// <summary>
    /// 侦查技能相关卡牌（发现隐藏线索、密门等）
    /// </summary>
    spotHidden,

    /// <summary>
    /// 聆听技能相关卡牌（辨识声音、察觉异常动静）
    /// </summary>
    listen,

    /// <summary>
    /// 心理学技能相关卡牌（分析人格、判断谎言）
    /// </summary>
    psychology,

    /// <summary>
    /// 神秘学技能相关卡牌（辨识神秘符号、仪式和魔法书）
    /// </summary>
    occult,

    /// <summary>
    /// 克苏鲁神话技能相关卡牌（了解神话生物、咒语和禁忌知识）
    /// </summary>
    cthulhuMythos,

    /// <summary>
    /// 考古学技能相关卡牌（鉴定古董、发掘遗址、研究消亡文明）
    /// </summary>
    archaeology,

    /// <summary>
    /// 历史技能相关卡牌（回忆地区历史、辨识古代物品）
    /// </summary>
    history,

    /// <summary>
    /// 信用评级技能相关卡牌（衡量财富、社会地位和资源获取能力）
    /// </summary>
    creditRating,

    /// <summary>
    /// 急救技能相关卡牌（紧急处理伤病、稳定濒死状态）
    /// </summary>
    firstAid,

    /// <summary>
    /// 医学技能相关卡牌（专业治疗、诊断疾病、外科处理）
    /// </summary>
    medicine,

    /// <summary>
    /// 机械维修技能相关卡牌（修理机械装置、制造简单物件）
    /// </summary>
    mechanicalRepair,

    /// <summary>
    /// 电气维修技能相关卡牌（改装或维修电气设备）
    /// </summary>
    electricalRepair,

    /// <summary>
    /// 电子学技能相关卡牌（维修电子设备、破解电子系统）
    /// </summary>
    electronics,

    /// <summary>
    /// 驾驶技能相关卡牌（操作特定交通工具，如汽车、飞机）
    /// </summary>
    drive,

    /// <summary>
    /// 闪避技能相关卡牌（躲避攻击和危险）
    /// </summary>
    dodge,

    /// <summary>
    /// 交涉技能相关卡牌（说服、谈判、获取信息）
    /// </summary>
    persuade,

    /// <summary>
    /// 潜行技能相关卡牌（隐蔽移动、避免被发现）
    /// </summary>
    stealth,

    /// <summary>
    /// 格斗技能相关卡牌（徒手或简易武器战斗）
    /// </summary>
    brawl,

    /// <summary>
    /// 射击技能相关卡牌（使用枪械进行攻击）
    /// </summary>
    firearms,

    /// <summary>
    /// 快速交谈技能相关卡牌（临时欺骗、机智应对）
    /// </summary>
    fastTalk,

    /// <summary>
    /// 锁匠技能相关卡牌（开锁、破解锁具）
    /// </summary>
    locksmith,

    /// <summary>
    /// 语言学技能相关卡牌（翻译文本、掌握多语言）
    /// </summary>
    linguistics,

    /// <summary>
    /// 乔装技能相关卡牌（伪装身份、改变外貌与行为）
    /// </summary>
    disguise,

    /// <summary>
    /// 驯兽技能相关卡牌（训练动物执行指令、建立服从关系）
    /// </summary>
    animalTraining,

    /// <summary>
    /// 表演技能相关卡牌（通过表演吸引注意、传递信息或伪装）
    /// </summary>
    performance,

    /// <summary>
    /// 天文学技能相关卡牌（观测星体、导航或解读天体神话）
    /// </summary>
    astronomy,

    /// <summary>
    /// 魅惑技能相关卡牌（通过魅力赢得好感、建立信任）
    /// </summary>
    charm,

    /// <summary>
    /// 攀爬技能相关卡牌（攀爬垂直表面、维持平衡）
    /// </summary>
    climb,

    /// <summary>
    /// 美术技能相关卡牌（创作或鉴赏艺术品、鉴定真伪）
    /// </summary>
    fineArt,

    /// <summary>
    /// 恐吓技能相关卡牌（通过威胁迫使他人服从）
    /// </summary>
    intimidate,

    /// <summary>
    /// 图书馆使用技能相关卡牌（高效查阅文献、定位关键信息）
    /// </summary>
    libraryUse,

    /// <summary>
    /// 精神分析技能相关卡牌（解析心理创伤、发现潜意识动机）
    /// </summary>
    psychoanalysis,

    /// <summary>
    /// 追踪技能相关卡牌（通过痕迹追踪目标行踪）
    /// </summary>
    track,
    /// <summary>
    /// 投掷技能相关卡牌（准确投掷武器或物品）
    /// </summary>
    throwing,
    npc,
    //-------------------------------------场景卡
    FogforestTown,//雾森镇
    //-------------------------------------空间卡
    NResidentArea,
    SResidentArea,
    WResidentArea,
    EResidentArea,
    CentralHospital,//中心医院
    MUniversity,//大学
    Museum,//博物馆
    Market,//市场
    WoodLand,//古木林场
    NpcD1,//npc
}