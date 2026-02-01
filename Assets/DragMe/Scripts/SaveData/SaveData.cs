using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class UniqueIdGenerator
{
    public int _nextId = 0;

    public int Next()
    {
        return ++_nextId;
    }
}

public class ConfigSaveData
{
    /// <summary>
    /// 主要的NPC
    /// </summary>
    public List<NpcCreateInf> mainNpcCfg;
    /// <summary>
    /// 子角色
    /// </summary>
    public List<NpcCreateInf> subNpcCfg;
    /// <summary>
    /// 所有的场景
    /// </summary>
    public List<SpaceCreatorRef> allSpacesCfg;
    /// <summary>
    /// NPC卡片配置
    /// </summary>
    public List<MonsterConfig> NpcCardsConfig = new();
    /// <summary>
    /// 场景配置
    /// </summary>
    public List<SpaceCardConfig> SpaceCardsConfig=new();

    [Button]
    public void CreateAllSpaces()
    {
        SpaceCardsConfig.Clear();
        foreach (var spaceCfg in allSpacesCfg)
        {
            SpaceCardsConfig.Add(spaceCfg.CreateCfg());
        }

        foreach (var spacecfg in allSpacesCfg)
        {
            var space = SpaceCardsConfig.Find(e => { return e.title == spacecfg.name;});
            if (space != null)
            {
                foreach (var next in spacecfg.spaces)
                {
                    var drawLine =space.ComponentCreators.Find(e => { return e is DrawLineComponentCreator;}) as DrawLineComponentCreator;
                    var paths = space.ComponentCreators.Find(e => { return e is PathComponentCreator; }) as PathComponentCreator;
                    var draw = new CardLineData();
                    draw.action = "";
                    draw.from = space;
                    var toPos = SpaceCardsConfig.Find(e => { return e.title == next.name;});
                    draw.to = toPos;
                    drawLine.CardLineDatas.Add(draw);
                    var pathData = new PathInfoCreator();
                    pathData.wasterTime = 1;
                    pathData.SpaceCardConfig = toPos;
                    paths.PathInfo.Add(pathData);
                }
            }
        }
    }
    [Button("创建场景卡片")]
    public void CreateAllSpaceCards()
    {
        foreach (var config in SpaceCardsConfig)
        {
            var sp = new SpaceCardCreateInfo();
            sp.cfg=config;
            GameFrameWork.Instance.gameConfig.CreateCard(sp);
        }

        foreach (var space  in GameFrameWork.Instance.data.saveFile.Space)
        {
            space.GetComponent<PathComponent>().Init();
        }
    }

    [Button]
    public void CreateNpcCards()
    {
        NpcCardsConfig.Clear();
        foreach (var mainNpc in mainNpcCfg)
        {
            NpcCardsConfig.Add(mainNpc.CreateCfg());
        }
        foreach (var subNpc in subNpcCfg)
        {
            NpcCardsConfig.Add(subNpc.CreateCfg());
        }
        ///对npc的解析
        foreach (var npc in NpcCardsConfig)
        {
            var cfg = mainNpcCfg.Find(e => { return e.name == npc.monsterName; });
            if (cfg == null)
            {
                cfg = subNpcCfg.Find(e => { return e.name == npc.monsterName;});
            }
            var relationship = npc.ComponentCreators.Find(e => { return e is RelationComponentCreator;}) as RelationComponentCreator;
            foreach (var other in cfg.relationships)
            {
                var relat = new RelationRecord();
                relat.RelationshipSummary = other.Value.relation;
                relat.AttitudeDescription = other.Value.attitude;
                relat.TargetNpcId = NpcCardsConfig.Find(e => { return e.monsterName == other.Key; }).GetID();
                relationship.Relations[other.Key] = relat;
            }
        }
    }
    /// <summary>
    /// 创建npc
    /// </summary>
    [Button("创建NPC卡片")]
    public void CreateNpcs()
    {
        foreach (var x in NpcCardsConfig)
        {
            var m = new MonsterCardCreateInfo();
            m.monsterId = x.monsterName;
            m.monsterConfig = x;
            var npc = GameFrameWork.Instance.gameConfig.CreateCard(m);
            npc.GetComponent<BelongComponent>().Init();
        }
    }
}

[Serializable]
public class SaveFile
{
    public long NowTime;
    public UniqueIdGenerator IdGenerator;
    public List<CardModel> cards;
    public List<TaskPanelModel> tasks;
    public List<SpaceCardModel> Space;
    /// <summary>
    /// npc角色
    /// </summary>
    public List<NpcCardModel> npcs;
    /// <summary>
    /// 网格卡
    /// </summary>
    public List<CellModel> CellModels;
    /// <summary>
    /// 社会圈子
    /// </summary>
    public SocialCircleConfig socialCircleConfig;
    [OdinSerialize]
    public Dictionary<string, IRegisterID> idMap;
    public List<WaitTimeNode> WaitTimeNodes;
    /// <summary>
    /// 配置的存储数据
    /// </summary>
    public ConfigSaveData ConfigSaveData;
    public void AddNpc(NpcCardModel npc)
    {
        cards.Add(npc);
        npcs.Add(npc);
    }

    public void AddCell(CellModel cellModel)
    {
        cards.Add(cellModel);
        CellModels.Add(cellModel);
    }

    public void AddCfgSaveData(Dictionary<string, NpcCreateInf> npc1,Dictionary<string, NpcCreateInf> npc2,List<SpaceCreatorRef> spaces)
    {
        if (ConfigSaveData == null)
        {
            ConfigSaveData = new ConfigSaveData();
        }

        ConfigSaveData.mainNpcCfg = npc1.Values.ToList();
        ConfigSaveData.subNpcCfg = npc2.Values.ToList();
        ConfigSaveData.allSpacesCfg = spaces;
    }
}

[CreateAssetMenu(fileName = "newSaveData", menuName = "SaveData/newSaveData")]
public class SaveData : SerializedScriptableObject
{
    [NonSerialized,OdinSerialize]
    public SaveFile saveFile;
    [Button]
    public void Save()
    {
        var filePath = "Assets/Resources/Saves" + "/tablesaveData.dat";
        Debug.Log(filePath);
        var json = SerializationUtility.SerializeValue<SaveFile>(saveFile, DataFormat.JSON);
        File.WriteAllBytes(filePath, json);
        Debug.Log("Data Saved: " + json);
    }
    [Button]
    public void Load()
    {
        var filePath = "Assets/Resources/Saves" + "/tablesaveData.dat";
        var json = File.ReadAllBytes(filePath);
        saveFile = SerializationUtility.DeserializeValue<SaveFile>(json, DataFormat.JSON);
    }
    /// <summary>
    /// 创建卡牌
    /// </summary>
    // [Button]
    // public void CreateCard(CardCreateInfo CardCreateInfo)
    // {
    //     saveFile.cards.Add(GameFrameWork.Instance.gameConfig.CardMap[CardCreateInfo.Belong()].CreateModel(CardCreateInfo));
    // }

    public void AddNpc(NpcCardModel npc)
    {
        saveFile.cards.Add(npc);
        saveFile.npcs.Add(npc);
    }
    
    [Button]
    public void InitIdMap()
    {
        saveFile.idMap.Clear();
        foreach (var x in saveFile.cards)
        {
            if (x.UID!=null)
            {
                saveFile.idMap[x.UID.id] = x;
            }
        }
    }
}
