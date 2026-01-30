using Sirenix.OdinInspector;

public enum ReproductionMethod
{
    //#region 自然无性繁殖（无需配偶，单一亲代直接产生子代）
    [LabelText("分裂繁殖（亲体直接分裂为两个或多个子代，如细菌、变形虫）")]
    Fission,

    [LabelText("出芽繁殖（亲体体表生出小芽体，成熟后脱离，如水螅、酵母菌）")]
    Budding,

    [LabelText("孢子繁殖（通过释放孢子萌发新个体，如真菌、蕨类植物）")]
    Sporulation,

    [LabelText("断裂繁殖（身体断裂后，每段重新长成完整个体，如蚯蚓、海星）")]
    Fragmentation,

    [LabelText("孤雌生殖（雌性无需雄性受精，直接产生后代，如蚜虫、某些蜥蜴）")]
    Parthenogenesis,


    //#region 自然有性繁殖（需配子结合，基因重组）
    [LabelText("雌雄异体受精（雌雄个体分别产生配子，体外/体内结合，如多数动物）")]
    SexualDimorphicFertilization,

    [LabelText("雌雄同体交配（同一生物兼具雌雄生殖器官，相互受精，如蜗牛、蚯蚓）")]
    HermaphroditicMating,

    [LabelText("性别转换繁殖（个体随环境转换性别后交配，如小丑鱼、某些鱼类）")]
    SequentialHermaphroditism,


    //#region 科幻生物繁殖（超越自然规则的机制）
    [LabelText("基因注入（将自身基因注入宿主细胞，改造宿主成为子代载体，如异形）")]
    GeneInjection,

    [LabelText("孢子寄生（孢子侵入宿主体内发育，成熟后破体而出，如《怪形》）")]
    ParasiticSporulation,

    [LabelText("机械复制（通过自身携带的工厂模块复制子代，如机械虫族）")]
    MechanicalReplication,

    [LabelText("能量分裂（能量体生命分裂出子能量团，独立后形成新个体）")]
    EnergySplitting,

    [LabelText("信息转录（将自身意识/数据写入介质，激活后成为新个体，如数字生命）")]
    DataTranscription,

    [LabelText("共生繁殖（两种生物结合后产生子代，缺一不可，如某些外星共生体）")]
    SymbioticReproduction,


    //#region 奇幻/异常繁殖（超自然或规则扭曲的方式）
    [LabelText("献祭诞生（通过牺牲生命/能量召唤或催生新个体，如恶魔、元素生物）")]
    SacrificialConjuring,

    [LabelText("概念衍生（从特定概念/情绪中自然诞生，如恐惧凝聚的暗影生物）")]
    ConceptualEmanation,

    [LabelText("环境结晶（在特定环境中由物质结晶形成，如宝石怪、冰元素）")]
    EnvironmentalCrystallization,

    [LabelText("时间回溯繁殖（个体自身回溯到幼体状态，本体与幼体共存，如某些时空生物）")]
    TemporalRegression,


    //#region 人工干预繁殖（由智慧文明设计的方式）
    [LabelText("克隆培育（通过体细胞复制产生基因相同的个体）")]
    Cloning,

    [LabelText("基因编辑繁殖（人工修改配子基因后结合，定向培育子代）")]
    GeneticallyEditedBreeding,

    [LabelText("集体意识分裂（从群体意识中分离出独立意识，赋予新躯体）")]
    CollectiveConsciousnessSplitting
}

public interface IReproductionFlag:ICardFlag
{
    
}