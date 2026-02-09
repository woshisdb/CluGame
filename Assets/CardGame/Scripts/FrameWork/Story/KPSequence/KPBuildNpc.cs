using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class KPBuildNpc
{
    /// <summary>
    /// 完善数据字典
    /// </summary>
    [Button("完善数据字典，丰富每个角色的行为动机")]
    public async Task GenerateAllStory()
    {
        var conds = KPSystem.Load<List<string>>("预期事件");
        var history = KPSystem.Load<string>("历史故事描述");
        var rawText = KPSystem.Load("模组精简");
        var typedDict = KPSystem.Load<Dictionary<string, CocDicItem>>("数据字典_typed");
        var conflicts = await KPStory.GptExtractWorldProcesses(history,conds,3);
        var teamDic = new Dictionary<string, string>();
        foreach (var dicItem in typedDict)
        {
            var value = dicItem.Value;
            if (value.type == "character")//如果是角色的话
            {
                var moreDetail = await KPNPCDetail.GPTGetNpcMoreDescription(rawText,dicItem.Key,value.description,conflicts);
                Debug.Log(moreDetail);
                teamDic[dicItem.Key] = moreDetail;
                var behave= await KPStory.GetNpcMainStory(dicItem.Key,value.description,3,moreDetail);
                value.description += "\n[*核心思想]" +behave;
            }
        }
        KPSystem.Save<Dictionary<string, CocDicItem>>("数据字典_typed",typedDict);
    }
    [Button("生成每个角色NPC信息")]
    public async void GenerateAllNPC()
    {
        var npcCreatorDic = new Dictionary<string, NpcCreateInf>();
        var typedDict = KPSystem.Load<Dictionary<string, CocDicItem>>("数据字典_typed");
        foreach (var dicItem in typedDict)
        {
            var value = dicItem.Value;
            if (value.type == "character")//如果是角色的话
            {
                var cf = await GameGenerate.CreateNpcInfo(dicItem.Key, dicItem.Value.description);
                cf.name = dicItem.Key;
                npcCreatorDic[dicItem.Key] = cf;
            }
        }

        GameFrameWork.Instance.data.saveFile.ConfigSaveData.mainNpcCfg = npcCreatorDic.Values.ToList();
        Debug.Log("11111");
    }
    [Button("生成所有场景")]
    public async void GenerateAllSpaces()
    {
        var typedDict = KPSystem.Load<Dictionary<string, CocDicItem>>("数据字典_typed");
        var npsCs = GameFrameWork.Instance.data.saveFile.ConfigSaveData.mainNpcCfg;
        var spaces = new List<SpaceCreatorRef>();
        foreach (var dicItem in npsCs)
        {
            var item = typedDict[dicItem.name];
            spaces = await GameGenerate.GenerateSpaces(item.description,dicItem,spaces);
            
            Debug.Log(11);
        }

        var cfgs = new List<SpaceCardConfig>();
        foreach (var x in spaces)
        {
            cfgs.Add(x.CreateCfg());
        }
        GameFrameWork.Instance.data.saveFile.ConfigSaveData.SpaceCardsConfig = cfgs;
        Debug.Log(11111);
    }
    [Button("生成场景地图")]
    public async void GenerateSpaceByDetails()
    {
        var rawText = KPSystem.Load("模组精简");
        var sps = GameFrameWork.Instance.data.saveFile.ConfigSaveData.SpaceCardsConfig;
        var data = new List<string>();
        foreach (var x in sps)
        {
            data.Add(x.title);
        }
        var ret = await KPSpaceGen.RebuildCocMapHierarchy(rawText,sps);
        Debug.Log(111);
    }
}