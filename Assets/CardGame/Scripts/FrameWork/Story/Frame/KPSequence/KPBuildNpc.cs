using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// 生成正式的角色配置信息
    /// </summary>
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
    /// <summary>
    /// 生成场景的原始数据和角色所处地点
    /// </summary>
    [Button("生成所有场景")]
    public async Task GenerateAllSpaces()
    {
        var typedDict = KPSystem.Load<Dictionary<string, CocDicItem>>("数据字典_typed");

        var npcPlaceDic = new Dictionary<string, string>();

        // ① 获取基础地形
        var basePlaceTxt = await GetPlaceTxt();
        var finalPlaceTxt = basePlaceTxt;

        foreach (var npcCfg in typedDict)
        {
            if (npcCfg.Value.type != "character")
                continue;

            // ② 检测 NPC 是否有缺失地点
            var addPlaceTxt = await KPSpaceGen.DetailSpaceInfo(finalPlaceTxt, npcCfg.Key, npcCfg.Value);

            // 如果返回“无新增地点”则跳过合并
            if (!string.IsNullOrEmpty(addPlaceTxt) && addPlaceTxt.Trim() != "无新增地点")
            {
                finalPlaceTxt = await KPSpaceGen.CombineSpaceInfo(addPlaceTxt, finalPlaceTxt);
            }
            
            Debug.Log("我----------"+npcCfg.Key+"\n"+finalPlaceTxt);
            // ③ 生成人物地点归属信息
            var personPlaceInfo = await KPSpaceGen.OutPersonPlaceInfo(finalPlaceTxt, npcCfg.Key, npcCfg.Value);

            npcPlaceDic[npcCfg.Key] = personPlaceInfo;
        }

        Debug.Log("=== 最终世界结构 ===");
        Debug.Log(finalPlaceTxt);

        Debug.Log("=== NPC 地点归属 ===");
        foreach (var kv in npcPlaceDic)
        {
            Debug.Log($"{kv.Key}:\n{kv.Value}");
        }
        KPSystem.Save("生成地图的原始数据",finalPlaceTxt);
        KPSystem.Save("NPC归属地点信息",npcPlaceDic);
    }
    [Button("细化场景信息")]
    public async Task GenerateMoreDetail()
    {
        var ret = await KPSpaceGen.GenerateMoreDetail();
        var realRet = RemoveDeletedNodes(ret);
        Debug.Log(realRet);
        KPSystem.Save("生成地图数据",realRet);
        var spaceNodes = await KPSpaceGen.GenerateSpaceNodesFromGPT(realRet);
        KPSystem.Save("生成SpaceNodes",spaceNodes);
        
    }
    public static string RemoveDeletedNodes(string mapText)
    {
        var lines = mapText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        var result = new List<string>();

        int? skipIndentLevel = null;

        foreach (var rawLine in lines)
        {
            var line = rawLine;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // 当前缩进层级（统计前导空格）
            int indentLevel = line.TakeWhile(Char.IsWhiteSpace).Count();

            // 如果当前在跳过模式
            if (skipIndentLevel.HasValue)
            {
                if (indentLevel > skipIndentLevel.Value)
                {
                    // 子节点，继续跳过
                    continue;
                }
                else
                {
                    // 退出跳过模式
                    skipIndentLevel = null;
                }
            }

            // 判断是否包含删除标记
            if (line.Contains("删除"))
            {
                skipIndentLevel = indentLevel;
                continue;
            }

            result.Add(line);
        }

        return string.Join(Environment.NewLine, result);
    }
    
    [Button("生成场景信息（结构化）")]
    public async void GenerateSpaceConfig()
    {
        var finalPlaceTxt = KPSystem.Load("生成地图的原始数据");

        if (string.IsNullOrEmpty(finalPlaceTxt))
        {
            Debug.LogError("地图文本为空");
            return;
        }

        var spaceNodes = await KPSpaceGen.GenerateSpaceNodesFromGPT(finalPlaceTxt);

        if (spaceNodes == null)
        {
            Debug.LogError("GPT 解析失败");
            return;
        }

        Debug.Log($"生成完成，共 {spaceNodes.Count} 个顶级节点");

        // GameFrameWork.Instance.data.saveFile.ConfigSaveData.SpaceNodes = spaceNodes;
        KPSystem.Save("生成的SpaceNode结构", spaceNodes);
    }
    
    [Button("生成场景地图")]
    public async void GenerateSpaceByDetails()
    {
        var rawText = KPSystem.Load("模组精简");
        var sps = KPSystem.Load<List<SpaceCardConfig>>("存储地图");
        // var sps = GameFrameWork.Instance.data.saveFile.ConfigSaveData.SpaceCardsConfig;
        var data = new List<string>();
        foreach (var x in sps)
        {
            data.Add(x.title);
        }
        var npcs = GameFrameWork.Instance.data.saveFile.ConfigSaveData.mainNpcCfg;
        var ret = await KPSpaceGen.GeneratePlayableMap(sps);
        var newret = await KPSpaceGen.ReThinkSpace(ret,npcs);
        var realRet = KPSpaceGen.BuildSpaceConfig(newret);
        GameFrameWork.Instance.data.saveFile.ConfigSaveData.SpaceCardsConfig = realRet;
    }
    
    [Button("获取可跑团层级地形结构")]
    public async Task<string> GetPlaceTxt()
    {
        var cocTxt = KPSystem.Load("模组精简");

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是《克苏鲁的呼唤（Call of Cthulhu）》模组的【跑团地形结构生成器】。

你不是在复述文本，而是在为 KP 生成【可用于跑团与系统建模的地形层级结构】。

你的输出将被直接用于：
- 地点节点生成
- NPC 行动落点
- 剧情触发位置绑定"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"
━━━━━━━━━━━━━━━━━━━━
【模组文本】
━━━━━━━━━━━━━━━━━━━━
{cocTxt}

━━━━━━━━━━━━━━━━━━━━
【你的目标】
━━━━━━━━━━━━━━━━━━━━
请将模组中的地点信息，整理为一个【严格层级化】的地形结构：

【唯一地区（X）】
  └─【唯一城镇 / 聚落（Y）】
      └─【功能区域】
          └─【子区域（可选）】
              └─【具体可跑团地点】
【唯一地区（Z）】
  └─【唯一城镇 / 聚落（W）】
      └─【功能区域】
          └─【子区域（可选）】
              └─【具体可跑团地点】

━━━━━━━━━━━━━━━━━━━━
【重要规则（必须遵守）】
━━━━━━━━━━━━━━━━━━━━
1. 必须列出所有NPC可能前往的地点
2. 如果有多个地区也要列出来
4. 地点必须是：
   - 调查员可能前往的
   - 或 NPC 可能出现 / 活动的
5. 不要添加模组文本中未出现的地点
6. 不要为了完整性补地点
7. 不要输出剧情、解释或分析

━━━━━━━━━━━━━━━━━━━━
【输出格式（纯文本，必须严格遵守）】
━━━━━━━━━━━━━━━━━━━━
使用以下结构示例（仅示例，内容请替换）：

X地区名：
- Y城镇名：
  - 功能区名：
    - 子区域名：
      - 具体地点名
    - 具体地点名
Z地区名...

━━━━━━━━━━━━━━━━━━━━
【输出要求】
━━━━━━━━━━━━━━━━━━━━
- 只输出结构化文本
- 不要代码块
- 不要解释
- 不要合并地点
- 保持层级清晰、稳定、可解析
"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        return result ?? string.Empty;
    }

}

public class MissingSpaceResult
{
    public List<MissingSpaceInfo> missingSpaces;
}

public class MissingSpaceInfo
{
    public string name;
    public string detail;
}
