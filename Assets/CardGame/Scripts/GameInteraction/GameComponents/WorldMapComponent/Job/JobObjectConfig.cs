using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum JobCategory
{
    [LabelText("农业")]
    Agriculture,

    [LabelText("工业")]
    Industry,

    [LabelText("服务业")]
    Service,

    [LabelText("医疗")]
    Medical,

    [LabelText("治安")]
    Security,

    [LabelText("调查")]
    Investigation,

    [LabelText("研究")]
    Research,

    [LabelText("秘学 / 邪术")]
    Occult
}

public enum JobTag
{
    // =========================
    // 🏠 空间 / 场所相关
    // =========================

    [LabelText("室内")]
    Indoor,

    [LabelText("室外")]
    Outdoor,

    [LabelText("公共场所")]
    PublicPlace,

    [LabelText("私人场所")]
    PrivatePlace,

    [LabelText("封闭空间")]
    Enclosed,

    // =========================
    // ⏰ 时间 / 节律
    // =========================

    [LabelText("白天")]
    Daytime,

    [LabelText("夜间")]
    NightOnly,

    [LabelText("深夜")]
    LateNight,

    // =========================
    // 👁 可见性 / 隐秘性（你非常需要）
    // =========================

    [LabelText("公开")]
    Public,

    [LabelText("隐秘")]
    Secret,

    [LabelText("难以察觉")]
    Subtle,

    [LabelText("引人注目")]
    EyeCatching,

    // =========================
    // ⚠ 风险 / 安全
    // =========================

    [LabelText("危险")]
    Dangerous,

    [LabelText("高风险")]
    HighRisk,

    [LabelText("致命")]
    Lethal,

    [LabelText("安全")]
    Safe,

    // =========================
    // 🧠 心理 / 精神
    // =========================

    [LabelText("精神压力")]
    MentalStrain,

    [LabelText("恐惧")]
    FearInducing,

    [LabelText("理智消耗")]
    SanityDrain,

    // =========================
    // 🕵️ 信息 / 调查
    // =========================

    [LabelText("调查行为")]
    Investigative,

    [LabelText("研究行为")]
    ResearchOriented,

    [LabelText("信息收集")]
    InformationGathering,

    [LabelText("误导信息")]
    Misinformation,

    // =========================
    // 🩸 怪奇 / 超自然
    // =========================

    [LabelText("超自然")]
    Occult,

    [LabelText("邪术")]
    DarkRitual,

    [LabelText("异界影响")]
    Otherworldly,

    [LabelText("禁忌")]
    Forbidden,

    // =========================
    // ⚖ 法律 / 社会反应
    // =========================

    [LabelText("合法")]
    Legal,

    [LabelText("非法")]
    Illegal,

    [LabelText("灰色行为")]
    GreyArea,

    [LabelText("引起官方注意")]
    AuthoritySensitive,

    // =========================
    // 🤝 社会关系
    // =========================

    [LabelText("需要信任")]
    TrustBased,

    [LabelText("小圈子")]
    SmallCircle,

    [LabelText("涉及派系")]
    FactionRelated,

    // =========================
    // 🔁 系统行为
    // =========================

    [LabelText("持续性")]
    Ongoing,

    [LabelText("一次性")]
    OneShot,

    [LabelText("可中断")]
    Interruptible,

    [LabelText("高优先级")]
    HighPriority
}
[Flags]
public enum WorkDays
{
    None      = 0,
    Monday    = 1 << 0,
    Tuesday   = 1 << 1,
    Wednesday = 1 << 2,
    Thursday  = 1 << 3,
    Friday    = 1 << 4,
    Saturday  = 1 << 5,
    Sunday    = 1 << 6
}
public class DailyWorkHours
{
    public int StartHours;
    public int EndHours;
}

public class WeeklyWorkTimeConfig
{
    public WorkDays WorkDays;
    public DailyWorkHours DailyHours = new();
}

/// <summary>
/// 工作收益配置
/// </summary>
public class WorkReward
{
    /// <summary>
    /// 基础金钱收益（每天 / 每次结算）
    /// </summary>
    public int Money;

    /// <summary>
    /// 经验值（职业经验 / 通用经验）
    /// </summary>
    public int Experience;

    /// <summary>
    /// 声望变化（正负皆可）
    /// </summary>
    public int Reputation;

    /// <summary>
    /// 技能成长（SkillId -> 增长值）
    /// </summary>
    public Dictionary<string, int> SkillGrowth = new();

    /// <summary>
    /// 理智 / 精神变化（克苏鲁系非常常用）
    /// </summary>
    public int SanityChange;

    /// <summary>
    /// 是否有概率获得额外奖励
    /// </summary>
    public ExtraRewardConfig ExtraReward;
}

public class ExtraRewardConfig
{
    /// <summary>
    /// 触发概率（0~1）
    /// </summary>
    public float Probability;

    /// <summary>
    /// 额外金钱
    /// </summary>
    public int BonusMoney;

    /// <summary>
    /// 额外物品ID
    /// </summary>
    public List<string> ItemIds = new();
}


/// <summary>
/// 工作风险配置
/// </summary>
public class WorkRisk
{
    /// <summary>
    /// 基础风险概率（0~1）
    /// </summary>
    public float RiskProbability;

    /// <summary>
    /// 身体伤害
    /// </summary>
    public int HealthDamage;

    /// <summary>
    /// 理智损失
    /// </summary>
    public int SanityDamage;

    /// <summary>
    /// 可能附加的状态效果
    /// </summary>
    public List<RiskStatusEffect> StatusEffects = new();

    /// <summary>
    /// 是否可能触发事件（剧情 / 战斗 / 调查）
    /// </summary>
    public bool CanTriggerEvent;
}
public class RiskStatusEffect
{
    /// <summary>
    /// 状态ID（如 Injured, Cursed, Wanted）
    /// </summary>
    public string EffectId;

    /// <summary>
    /// 持续天数
    /// </summary>
    public int DurationDays;

    /// <summary>
    /// 触发概率（0~1）
    /// </summary>
    public float Probability;
}


[CreateAssetMenu(fileName = "工作设置", menuName = "配置/工作配置")]
public class JobObjectConfig:SerializedScriptableObject
{
    public JobCategory Category;
    public List<JobTag> Tag;
    public string Name;
    public string Description;
    public int Prestige;              // 社会地位（-100 ~ +100）
    public WeeklyWorkTimeConfig WeeklyWorkTimeConfig = new ();
    private WorkDays ToWorkDay(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Monday    => WorkDays.Monday,
            DayOfWeek.Tuesday   => WorkDays.Tuesday,
            DayOfWeek.Wednesday => WorkDays.Wednesday,
            DayOfWeek.Thursday  => WorkDays.Thursday,
            DayOfWeek.Friday    => WorkDays.Friday,
            DayOfWeek.Saturday  => WorkDays.Saturday,
            DayOfWeek.Sunday    => WorkDays.Sunday,
            _ => WorkDays.None
        };
    }
    public bool IsWorkingNow(WeeklyWorkTimeConfig config)
    {
        DateTime now = GameTimeManager.ToDateTime(GameFrameWork.Instance.GameTimeManager.CurrentTime);
        int hour = now.Hour;

        bool todayIsWorkDay =
            config.WorkDays.HasFlag(ToWorkDay(now.DayOfWeek));

        int start = config.DailyHours.StartHours;
        int end   = config.DailyHours.EndHours;

        // 非跨天
        if (end > start)
        {
            return todayIsWorkDay &&
                   hour >= start &&
                   hour < end;
        }

        // 跨天（夜班）
        bool yesterdayIsWorkDay =
            config.WorkDays.HasFlag(
                ToWorkDay(now.AddDays(-1).DayOfWeek)
            );

        return
            (todayIsWorkDay && hour >= start) ||
            (yesterdayIsWorkDay && hour < end);
    }
    public long GetMinutesUntilNextWorkStart(WeeklyWorkTimeConfig config)
    {
        DateTime now = GameTimeManager.ToDateTime(GameFrameWork.Instance.GameTimeManager.CurrentTime);

        for (int i = 0; i < 7; i++)
        {
            DateTime day = now.Date.AddDays(i);

            if (!config.WorkDays.HasFlag(ToWorkDay(day.DayOfWeek)))
                continue;

            DateTime startTime =
                day.AddHours(config.DailyHours.StartHours);

            // 今天但已经过了开始时间
            if (i == 0 && now >= startTime)
                continue;

            return (long)(startTime - now).TotalMinutes;
        }

        return -1;
    }

}