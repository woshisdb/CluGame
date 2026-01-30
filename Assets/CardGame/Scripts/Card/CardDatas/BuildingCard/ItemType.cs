using Sirenix.OdinInspector;

public enum ItemType
{
    // =====================================================
    // 自然原材料（1000–1999，4位精细）
    // =====================================================

    // ---- 木材系 1000–1099 ----
    [LabelText("原木")] Log = 1000,
    [LabelText("硬木原木")] HardwoodLog = 1001,
    [LabelText("软木原木")] SoftwoodLog = 1002,

    // ---- 石料系 1100–1199 ----
    [LabelText("石块")] StoneBlock = 1100,
    [LabelText("花岗岩")] Granite = 1101,
    [LabelText("大理石")] Marble = 1102,

    // ---- 土壤 / 非金属矿 1200–1299 ----
    [LabelText("黏土")] Clay = 1200,
    [LabelText("沙子")] Sand = 1201,
    [LabelText("石灰石")] Limestone = 1202,

    // ---- 水资源 1300–1399 ----
    [LabelText("淡水")] FreshWater = 1300,

    // ---- 金属矿石 1400–1499 ----
    [LabelText("铁矿石")] IronOre = 1400,
    [LabelText("铜矿石")] CopperOre = 1401,
    [LabelText("银矿石")] SilverOre = 1402,
    [LabelText("金矿石")] GoldOre = 1403,
    [LabelText("铅矿")] LeadOre = 1404,

    // ---- 能源矿物 1500–1599 ----
    [LabelText("煤炭")] Coal = 1500,

    // ---- 其他天然材料 1600–1699 ----
    [LabelText("生丝")] RawSilk = 1600,
    [LabelText("天然树脂")] NaturalResin = 1601,


    // =====================================================
    // 农业 / 畜牧（2000–2999）
    // =====================================================

    // ---- 粮食 ----
    [LabelText("小麦")] Wheat = 2000,
    [LabelText("稻米")] Rice = 2001,
    [LabelText("玉米")] Corn = 2002,

    // ---- 经济作物 ----
    [LabelText("啤酒花")] Hops = 2010,
    [LabelText("甘蔗")] SugarCane = 2011,
    [LabelText("棉花")] Cotton = 2012,
    [LabelText("亚麻")] Flax = 2013,

    // ---- 蔬菜 / 水果 ----
    [LabelText("土豆")] Potato = 2100,
    [LabelText("胡萝卜")] Carrot = 2101,
    [LabelText("苹果")] Apple = 2200,
    [LabelText("葡萄")] Grape = 2201,

    // ---- 畜牧 ----
    [LabelText("生牛肉")] RawBeef = 2300,
    [LabelText("生猪肉")] RawPork = 2301,
    [LabelText("生鸡肉")] RawChicken = 2302,

    [LabelText("羊毛")] Wool = 2310,
    [LabelText("生皮")] RawHide = 2311,
    [LabelText("动物脂肪")] AnimalFat = 2312,
    [LabelText("兽骨")] AnimalBone = 2313,

    [LabelText("牛奶")] Milk = 2320,
    [LabelText("鸡蛋")] Egg = 2321,

    // ---- 药用 ----
    [LabelText("药用草")] MedicinalHerb = 2400,


    // =====================================================
    // 初级加工 / 中间品（3000–3999）
    // =====================================================

    // ---- 木制 ----
    [LabelText("木板")] Plank = 30000,
    [LabelText("木梁")] WoodenBeam = 30010,
    [LabelText("木柄")] WoodenHandle = 30020,

    // ---- 建材 ----
    [LabelText("砖块")] Brick = 31000,
    [LabelText("玻璃")] Glass = 31010,

    // ---- 金属锭 ----
    [LabelText("铁锭")] IronIngot = 32000,
    [LabelText("铜锭")] CopperIngot = 32010,
    [LabelText("银锭")] SilverIngot = 32020,
    [LabelText("金锭")] GoldIngot = 32030,
    [LabelText("铅锭")] LeadIngot = 32040,
    [LabelText("钢锭")] SteelIngot = 32050,

    // ---- 食品中间品 ----
    [LabelText("面粉")] Flour = 33000,
    [LabelText("食用油")] CookingOil = 33010,
    [LabelText("糖")] Sugar = 33020,

    // ---- 纺织 / 皮革 ----
    [LabelText("布料")] Fabric = 34000,
    [LabelText("皮革")] Leather = 34010,

    // ---- 化工 ----
    [LabelText("燃料")] Fuel = 35000,
    [LabelText("润滑油")] Lubricant = 35010,
    [LabelText("火药")] Gunpowder = 35020,

    [LabelText("酒精")] AlcoholSolvent = 36000,
    [LabelText("蒸馏水")] DistilledWater = 36010,

        [LabelText("石灰")] Lime = 37000,
    [LabelText("纸张")] Paper = 37100,

    // =====================================================
    // 食品 / 饮品（40000–49990）
    // =====================================================

    [LabelText("面包")] Bread = 40000,
    [LabelText("米饭")] CookedRice = 40010,

    [LabelText("烤牛肉")] RoastedBeef = 41000,
    [LabelText("烤猪肉")] RoastedPork = 41010,
    [LabelText("烤鸡")] RoastedChicken = 41020,
    [LabelText("炖菜")] Stew = 41030,

    [LabelText("蛋糕")] Cake = 42000,

    [LabelText("啤酒")] Beer = 43000,
    [LabelText("葡萄酒")] Wine = 43010,
    [LabelText("烈酒")] Spirits = 43020,


    // =====================================================
    // 生活用品（50000–59990）
    // =====================================================

    [LabelText("粗布衣")] SimpleClothes = 50000,
    [LabelText("皮革衣物")] LeatherClothes = 50010,

    [LabelText("木制家具")] WoodenFurniture = 51000,
    [LabelText("金属家具")] MetalFurniture = 51010,

    [LabelText("肥皂")] Soap = 52000,
    [LabelText("油灯")] OilLamp = 52010,
    [LabelText("酒桶")] Barrel = 52020,
    [LabelText("书籍")] Book = 52030,

    // =====================================================
    // 工具 / 工业制品（60000–69990）
    // =====================================================

    [LabelText("铁制工具")] IronTool = 60000,
    [LabelText("农具")] FarmingTool = 60010,
    [LabelText("矿工工具")] MiningTool = 60020,

    [LabelText("金属紧固件")] MetalFastener = 61000,
    [LabelText("机械零件")] MachinePart = 61010,
    [LabelText("电力")] Electricity = 62000,


    // =====================================================
    // 建筑材料（70000–79990）
    // =====================================================

    [LabelText("建筑木料")] ConstructionTimber = 70000,
    [LabelText("建筑砖材")] ConstructionBrick = 70010,
    [LabelText("钢梁")] SteelBeam = 70020,


    // =====================================================
    // 医疗 / 化学（80000–89990）
    // =====================================================

    [LabelText("草药药剂")] HerbalMedicine = 80000,
    [LabelText("止痛药")] Painkiller = 80010,
    [LabelText("医疗器具")] MedicalInstrument = 80020,


    // =====================================================
    // 武器 / 军需（90000–99990）
    // =====================================================

    [LabelText("步枪")] Rifle = 90000,
    [LabelText("手枪")] Pistol = 90010,

    [LabelText("弹壳")] BulletCasing = 90100,
    [LabelText("底火")] Primer = 90110,
    [LabelText("弹药")] Ammunition = 90120,

    [LabelText("皮甲")] LeatherArmor = 91000,
    [LabelText("金属护甲")] MetalArmor = 91010,


    // =====================================================
    // 贵重 / 奢侈品（100000–109990）
    // =====================================================

    [LabelText("宝石")] Gemstone = 100000,
    [LabelText("金戒指")] GoldRing = 100010,
    [LabelText("银项链")] SilverNecklace = 100020,
    [LabelText("艺术品")] Artwork = 100030,


    // =====================================================
    // 非法 / 灰色商品（110000–119990）
    // =====================================================

    [LabelText("鸦片")] Opium = 110000,
    [LabelText("私酒")] BootlegAlcohol = 110010,
    [LabelText("走私武器")] SmuggledWeapons = 110020,
    [LabelText("赃物")] StolenGoods = 110030,

}
