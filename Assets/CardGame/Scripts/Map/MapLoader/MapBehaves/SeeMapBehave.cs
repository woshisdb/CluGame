/// <summary>
/// 查看这个行为,用于探索这个物品或场景是不是
/// </summary>
public class SeeMapBehave:MapBehave
{
    /// <summary>
    /// 看的对象
    /// </summary>
    public SeeComponent fromObj;
    /// <summary>
    /// 被看的对象
    /// </summary>
    public CanBeSeeComponent toObj;
    public SeeMapBehave(MapLoader map, SeeComponent fromObj, CanBeSeeComponent toObj) : base(map)
    {
        this.fromObj = fromObj;
        this.toObj = toObj;
    }

    public override void Run()
    {
        map.mapSeeSystem.See(this);
    }
}
/// <summary>
/// 信息或目标的隐藏程度。
/// 用于观察、探索、陷阱、线索、敌人的隐匿性判定。
/// </summary>
public enum HideLevel
{
    /// <summary>
    /// 完全显眼：不用检定也能看见
    /// </summary>
    Obvious = 0,
    //普通成功
    /// <summary>
    /// 稍微隐藏：普通观察即可发现（SimpleSee 可看到）
    /// </summary>
    Slight = 1,
    //困难
    /// <summary>
    /// 中度隐藏：需要特定查看类型才能看到（例如危险、知识）
    /// </summary>
    Moderate = 2,
    //极难
    /// <summary>
    /// 高度隐藏：必须成功检定，且要相应的 SeeType 才行
    /// </summary>
    Hidden = 3
}


public enum SeeType
{
    /// <summary>
    /// 简单查看周围环境（普通观察）
    /// 例如：房间布局、桌椅、电灯亮不亮
    /// </summary>
    SimpleSee,

    /// <summary>
    /// 搜寻危险物品、威胁、敌人潜伏
    /// 例如：陷阱、武器、异常动静、敌意目光
    /// </summary>
    Dangerous,

    /// <summary>
    /// 搜寻知识类信息
    /// 例如：书籍、符号、档案、仪式痕迹
    /// </summary>
    Knowledge,

    /// <summary>
    /// 搜寻隐藏物品、暗格、密门
    /// 例如：藏匿的箱子、地板缝隙、地下室入口
    /// </summary>
    HiddenObject,

    /// <summary>
    /// 搜寻行动痕迹
    /// 例如：脚印、血迹、拖拽痕迹、破损痕迹
    /// </summary>
    Trace,

    /// <summary>
    /// 搜寻超自然、诡异或无法解释的现象
    /// 例如：幻影、错位的空间、扭曲阴影
    /// </summary>
    OccultOrEldritch,

    /// <summary>
    /// 察觉是否有人监视、尾随、暗处观察
    /// 例如：偷窥者、潜伏者、黑影
    /// </summary>
    Surveillance,

    /// <summary>
    /// 察觉空间中的气味、声音、空气变化
    /// 例如：腐烂气味、仪式低语、机械声
    /// （偏向“其他感官”）
    /// </summary>
    Sensory,

    /// <summary>
    /// 搜寻心理或情绪线索
    /// 例如：氛围压迫、被盯着的感觉、精神污染
    /// （适用于恐怖氛围构建）
    /// </summary>
    Psychological,

    /// <summary>
    /// 专注细节分析（需要更高成功等级）
    /// 例如：材料微痕、笔迹、化学残留
    /// </summary>
    FineDetail,
    /// <summary>
    /// 发现噪音
    /// </summary>
    FindNoise,
}
