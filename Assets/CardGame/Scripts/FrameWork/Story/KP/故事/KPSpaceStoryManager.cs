using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class KPSpaceStoryManager
{
    public string context { get; private set; }
    public List<string> availableNpcs { get; private set; } = new List<string>();
    public Dictionary<string, NpcCardModel> sceneNpcs = new Dictionary<string, NpcCardModel>();
    public Dictionary<string, GptChatSession> npcChatSessions = new Dictionary<string, GptChatSession>();
    public GptChatSession narratorSession;

    /// <summary>
    /// 世界地图管理器引用
    /// </summary>
    public KPWorldMapManager worldMapManager;
    
    /// <summary>
    /// 场景空间管理器引用（管理物品和状态）
    /// </summary>
    public KPPlaceSpaceManager placeSpaceManager;
 
    /// <summary>
    /// 这里的重要信息有哪些
    /// </summary>
    public string importantThings { get; private set; }

    /// <summary>
    /// 已经发现的信息
    /// </summary>
    public string hasFindThings { get; private set; }

    /// <summary>
    /// 模组背景文本（用于生成NPC和重要信息）
    /// </summary>
    public string cocText { get; private set; }

    /// <summary>
    /// 初始化 KPSpaceStoryManager
    /// </summary>
    /// <param name="context">场景描述</param>
    /// <param name="cocText">模组背景文本</param>
    /// <param name="worldMapManager">世界地图管理器</param>
    public void Init(string context, string cocText = null, KPWorldMapManager worldMapManager = null)
    {
        this.context = context;
        this.cocText = cocText;
        this.worldMapManager = worldMapManager;
        this.placeSpaceManager = worldMapManager?.currentSpace?.placeSpaceManager;
        this.availableNpcs = new List<string>();
        this.importantThings = string.Empty;
        this.hasFindThings = string.Empty;
        this.sceneNpcs = new Dictionary<string, NpcCardModel>();
        this.npcChatSessions = new Dictionary<string, GptChatSession>();
        this.narratorSession = null;
        
        Debug.Log($"KPSpaceStoryManager 初始化完成: {context?.Substring(0, Math.Min(20, context.Length))}...");
    }
 
    /// <summary>
    /// 初始化并生成场景信息（异步）
    /// </summary>
    public async Task InitAndGenerateInfo(string context, string cocText = null, KPWorldMapManager worldMapManager = null)
    {
        Init(context, cocText, worldMapManager);
        
        // 生成场景信息
        await GenerateSceneInfo();
    }

    [Button("开始故事")]
    public async Task StartSpaceStory()
    {
        try
        {
        if (string.IsNullOrEmpty(context))
        {
            Debug.LogError("Context is empty!");
            return;
        }

        if (GameFrameWork.Instance.KP != null && GameFrameWork.Instance.KP.kpWorldManager != null)
        {
            worldMapManager = GameFrameWork.Instance.KP.kpWorldManager;
            worldMapManager.InitWorldMap();
        }

        var player = GameFrameWork.Instance.playerManager.nowPlayer;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        var playerChat = player.GetComponent<ChatComponent>();
        if (playerChat == null)
        {
            Debug.LogError("ChatComponent not found on player!");
            return;
        }

        // 调用当前场景的 Enter
        if (worldMapManager != null && worldMapManager.currentSpace != null)
        {
            worldMapManager.currentSpace.Enter(player);
        }

        ParseSceneNpcs();

        if (sceneNpcs.Count == 0)
        {
            Debug.LogWarning("No NPCs found in scene!");
        }

        InitNarratorSession();

        foreach (var npcPair in sceneNpcs)
        {
            InitNpcSession(npcPair.Key, npcPair.Value);
        }
        
        GameFrameWork.Instance.ChatPanel.Close();
        GameFrameWork.Instance.ChatPanel.Init(playerChat, null, new KPStoryListener(async e =>
        {
            e.panel.submitBtn.gameObject.SetActive(false);
            await HandleUserInput(e);
            e.panel.submitBtn.gameObject.SetActive(true);
        }, () => { }));

        if (worldMapManager != null && worldMapManager.currentSpace != null)
        {
            var spaceName = worldMapManager.currentSpace.space.title;
            GameFrameWork.Instance.ChatPanel.SetPlace(spaceName);
        }

        if (availableNpcs != null && availableNpcs.Count > 0)
        {
            GameFrameWork.Instance.ChatPanel.UpdateDropdownOptions(availableNpcs);
        }
        
        // 生成当前场景的NPC和重要信息
        await GenerateSceneInfo();
        
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
    }

    /// <summary>
    /// 生成当前场景的NPC和重要信息
    /// </summary>
    private async Task GenerateSceneInfo()
    {
        // 如果没有cocText，则尝试从KPWorldStoryManager获取
        if (string.IsNullOrEmpty(cocText) && GameFrameWork.Instance.KP?.KpWorldStoryManager != null)
        {
            cocText = KPSystem.Load("模组精简");
        }

        if (!string.IsNullOrEmpty(cocText))
        {
            await GenerateAvailableNpcs(cocText);
            await GenerateImportantThings(cocText);
        }
    }

    private async Task GenerateAvailableNpcs(string cocText)
    {
        var characterNpcs = GameFrameWork.Instance.playerManager.allNpc
            .Select(kv => kv.npcId)
            .ToList();

        var schema = GptSchemaBuilder.BuildSchema(typeof(AvailableNpcsResult));

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【可对话角色提取器】。
 
你的职责是：
从模组文本中提取【调查员可以与之对话的角色】。
 
规则：
- 角色必须是模组中明确存在的
- 角色必须在当前场景中出现
- 包含 KP（主持人）作为可对话对象
- 不包含普通背景角色"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【当前场景描述】
{context}

【已知角色列表】
{string.Join(", ", characterNpcs)}

【你的任务】

从当前场景中提取【调查员可以与之对话的角色】。

角色列表必须包含：
- KP（克苏鲁跑团的主持人）
- 场景中出现的其他可对话角色

注意：
- 你的返回角色名字必须与已知角色列表的名字相同

输出要求：
- 返回 JSON 对象
- 只输出 JSON，不输出其他内容

示例格式：
{{
  ""npcs"": [""KP"", ""角色A"", ""角色B""]
}}"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT<AvailableNpcsResult>(messages);

        availableNpcs = result?.npcs ?? new List<string>();
    }

    private async Task GenerateImportantThings(string cocText)
    {
        var sceneName = worldMapManager?.currentSpace?.space?.title ?? "未知场景";

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【场景重要信息提取器】。

你的职责是：
从【指定场景】中提取【调查员在该场景可能发现的重要信息】。

规则：
- 信息必须是可观察的、具体的
- 信息必须与该场景相关
- 不生成背景介绍或氛围描述
- 不生成过于细小的细节"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【CoC 模组原文】
{cocText}

【场景名称】
{sceneName}

【场景描述】
{context}

【你的任务】

从【场景描述】中提取【调查员在该场景可能发现的重要信息】。

重要信息应包含：
- 该场景中的异常现象
- 该场景中的关键线索或物品
- 该场景中角色的重要特征
- 该场景中可能的威胁或危险

输出要求：
- 只输出纯文本
- 不使用 JSON
- 不使用编号或列表
- 直接输出重要信息描述

示例格式：
墙壁上似乎有模糊的污渍，像是某种液体留下的痕迹。嫌疑人B看起来紧张不安，手指不停地敲击桌面。审讯室的灯光昏暗，空气中弥漫着陈旧的烟草味。"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        importantThings = result ?? string.Empty;
        hasFindThings = string.Empty;
    }

    private void ParseSceneNpcs()
    {
        sceneNpcs.Clear();
        var allNpcs = GameFrameWork.Instance.playerManager.allNpc;
        foreach (var x in availableNpcs)
        {
            var npc = allNpcs.Find(n => n.npcId == x);
            if (npc != null && !sceneNpcs.ContainsKey(x))
            {
                sceneNpcs[x] = npc;
            }
        }
    }

    private void InitNarratorSession()
    {
        var prompt = "你是《克苏鲁的呼唤》跑团的【KP（主持人）画外音解说】。\n" +
                     "\n" +
                     "你的职责是：\n" +
                     "- 描述场景氛围、环境变化\n" +
                     "- 旁白式的剧情推进\n" +
                     "- 回应玩家的非对话性输入\n" +
                     "⚠️ 重要约束：\n" +
                     "- 你不扮演任何具体角色\n" +
                     "- 你只做客观的、氛围化的描述\n" +
                     "- 不推进剧情，不替你做决定\n" +
                     "- 使用第三人称、描述性语言\n" +
                     "\n" +
                     "场景背景" +
                     context + "\n";
        
        // 添加场景状态（物品、环境等）
        if (placeSpaceManager != null)
        {
            prompt += "\n" + placeSpaceManager.GenerateSceneStateDescription() + "\n";
        }
        
        prompt += "【输出格式要求】\n" +
                  "你必须严格按以下 JSON 输出：\n" +
                  "{\n" +
                  "  \"message\": \"你的画外音描述内容\"\n" +
                  "}";

        narratorSession = new GptChatSession(prompt);
    }

    private void InitNpcSession(string npcName, NpcCardModel npc)
    {
        var npcTitle = npc.GetTitle() ?? npc.npcId ?? "未知角色";
        var npcDescription = npc.GetDescription() ?? "";
        
        var npcInfo = $"角色名称: {npcTitle}\n" +
                      $"角色ID: {npc.npcId}\n" +
                      $"角色描述: {npcDescription}\n";
        
        if (npc.NpcPhyInfo != null)
        {
            npcInfo += $"性别: {npc.NpcPhyInfo.sex}\n" +
                       $"年龄: {npc.NpcPhyInfo.age}\n";
        }
        
        if (npc.NpcThink != null && npc.NpcThink.npcThinks != null)
        {
            var thinkInfo = npc.NpcThink.npcThinks.GetType().GetProperties()
                .Select(p => $"{p.Name}: {p.GetValue(npc.NpcThink.npcThinks)}")
                .ToList();
            if (thinkInfo.Any())
            {
                npcInfo += $"性格特征: {string.Join(", ", thinkInfo)}\n";
            }
        }
        
        // 获取NPC当前任务
        var nowTaskComponent = npc.GetComponent<NowTaskComponent>();
        if (nowTaskComponent != null && nowTaskComponent.SupplyTask != null)
        {
            var taskType = nowTaskComponent.GetTaskType();
            npcInfo += $"当前任务: {taskType}\n";
        }
        
        // 构建完整提示
        var prompt = $"你是一个《克苏鲁的呼唤》跑团的【NPC对话生成器】。\n" +
                     "你的职责是：根据提供的 NPC 设定，生成该 NPC 在当前情境下的一句角色化回应，并用 JSON 输出。\n" +
                     "\n" +
                     npcInfo +
                     "场景描述: " + context + "\n";
        
        // 添加场景状态（物品、环境等）
        if (placeSpaceManager != null)
        {
            prompt += "\n" + placeSpaceManager.GenerateSceneStateDescription() + "\n";
        }
        
        // 添加CoC文本背景
        if (!string.IsNullOrEmpty(cocText))
        {
            prompt += "\n【CoC 模组背景】\n" + cocText + "\n";
        }
        
        prompt += "\n" +
                  "⚠️ 重要约束：\n" +
                  "- 你必须扮演这个 NPC，根据他的角色设定来回应\n" +
                  "- 保持角色的性格特点和行为（如年龄、性别、性格特征）\n" +
                  "- 如果NPC有当前任务，回应应该围绕任务展开\n" +
                  "- 回应要符合CoC模组的剧情背景和当前场景\n" +
                  "- 回应要考虑场景中的物品和环境状态\n" +
                  "- 你只负责生成 NPC 的回应，并将其格式化为 JSON\n" +
                  "- 除 JSON 外，不得输出任何内容\n" +
                  "\n" +
                  "【输出格式要求】\n" +
                  "你必须严格按以下 JSON 输出：\n" +
                  "{\n" +
                  "  \"message\": \"NPC 的一句角色化回应内容\"\n" +
                  "}";

        npcChatSessions[npcName] = new GptChatSession(prompt);
    }


    private async Task HandleUserInput(ChatInput input)
    {
        var userStr = input.Content.Trim();
        var selectedNpc = input.SelectedNpcName;

        if (!string.IsNullOrEmpty(selectedNpc) && selectedNpc != "KP")
        {
            await HandleNpcDialogue(selectedNpc, userStr, input);
        }
        else
        {
            await HandleFreeInput(userStr, input);
        }
    }

    private async Task HandleNpcDialogue(string npcName, string userStr, ChatInput input)
    {
        if (sceneNpcs.ContainsKey(npcName))
        {
            var npc = sceneNpcs[npcName];
            var npcChat = npc.GetComponent<ChatComponent>();

            if (npcChat != null && npcChatSessions.ContainsKey(npcName))
            {
                var session = npcChatSessions[npcName];

                var constrain = "。【硬性规则 - 不可违背】\\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\\n你不能输出任何 JSON 以外的内容。" +
                                GptSchemaBuilder.BuildSchema(typeof(ChatContext));
                var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(session, userStr,
                    constrain);

                session.AddChatHistory("NPC", evt.message);
                input.panel.AddMessage($"【{npcName}】{evt.message}");

                await CheckForNewInformation(npcName, userStr, evt.message);
            }
            else
            {
                input.panel.AddMessage($"【系统】无法与 {npcName} 对话，该 NPC 没有 ChatComponent");
            }
        }
        else
        {
            input.panel.AddMessage($"【系统】场景中不存在角色：{npcName}");
        }
    }

    private async Task HandleFreeInput(string userStr, ChatInput input)
    {
        if (RAParser.TryParse(userStr, out var raCommand))
        {
            await HandleRACommand(raCommand, input);
            return;
        }

        if (worldMapManager != null)
        {
            var moveIntent = await worldMapManager.DetectMoveIntent(userStr);
            if (moveIntent.wantsToMove)
            {
                input.panel.AddMessage($"【系统】正在前往｜{moveIntent.targetLocation}...");
                
                // 获取当前玩家
                var player = GameFrameWork.Instance.playerManager.nowPlayer;
                
        // 调用旧场景的 Exit
        if (worldMapManager.currentSpace != null && player != null)
        {
            worldMapManager.currentSpace.Exit(player);
        }
        
        var newStoryManager = await worldMapManager.SwitchToLocation(
            moveIntent.targetLocation, 
            context
        );
        
        // 调用新场景的 Enter
        if (worldMapManager.currentSpace != null && player != null)
        {
            worldMapManager.currentSpace.Enter(player);
        }
                
                GameFrameWork.Instance.KP.KpWorldStoryManager.SetNowSpaceManager(newStoryManager);
                await newStoryManager.StartSpaceStory();
                if (newStoryManager != null)
                {
                    GameFrameWork.Instance.ChatPanel.AddMessage($"【系统】已到达｜{moveIntent.targetLocation}，故事继续...");
                    return;
                }
            }
        }

        var parsedInput = ParseUserInput(userStr);

        if (parsedInput.IsDialogue)
        {
            if (sceneNpcs.ContainsKey(parsedInput.TargetNpc))
            {
                var npc = sceneNpcs[parsedInput.TargetNpc];
                var npcChat = npc.GetComponent<ChatComponent>();

                if (npcChat != null && npcChatSessions.ContainsKey(parsedInput.TargetNpc))
                {
                    var session = npcChatSessions[parsedInput.TargetNpc];

                    var constrain = "。【硬性规则 - 不可违背】\\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\\n你不能输出任何 JSON 以外的内容。" +
                                    GptSchemaBuilder.BuildSchema(typeof(ChatContext));
                    var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(session,
                        parsedInput.Message, constrain);

                    session.AddChatHistory("NPC", evt.message);
                    input.panel.AddMessage($"【{parsedInput.TargetNpc}】{evt.message}");

                    await CheckForNewInformation(parsedInput.TargetNpc, parsedInput.Message, evt.message);
                }
                else
                {
                    input.panel.AddMessage($"【系统】无法与 {parsedInput.TargetNpc} 对话，该 NPC 没有 ChatComponent");
                }
            }
            else
            {
                input.panel.AddMessage($"【系统】场景中不存在角色：{parsedInput.TargetNpc}");
            }
        }
        else
        {
            var checkResult = await CheckPlayerAction(userStr, input);
            
            narratorSession.AddChatHistory("Player", userStr);

            var constrain = "。【硬性规则 - 不可违背】\\n你是个Json格式输出工具，你每一次回复都【必须】【只能】输出合法 JSON。\\n你不能输出任何 JSON 以外的内容。" +
                            GptSchemaBuilder.BuildSchema(typeof(ChatContext));
            
            if (checkResult != null)
            {
                var outcomePrompt = BuildOutcomePrompt(checkResult);
                constrain += outcomePrompt;
            }
            
            var evt = await GameFrameWork.Instance.GptSystem.ChatInSession<ChatContext>(narratorSession, userStr,
                constrain);

            narratorSession.AddChatHistory("Narrator", evt.message);
            input.panel.AddMessage($"【画外音】{evt.message}");

            await CheckForNewInformation("画外音", userStr, evt.message);
        }
    }

    private async Task CheckForNewInformation(string target, string playerInput, string npcResponse)
    {
        if (string.IsNullOrEmpty(importantThings))
            return;

        var messages = new[]
        {
            new QwenChatMessage
            {
                role = "system",
                content =
                    @"你是一个《克苏鲁的呼唤（Call of Cthulhu）》模组的【新信息发现器】。

你的职责是：
判断对话中是否发现了【重要信息】。

规则：
- 只判断，不创作
- 基于已知的 importantThings 进行判断
- 不重复已发现的信息"
            },
            new QwenChatMessage
            {
                role = "user",
                content =
                    $@"【已知重要信息列表】
{importantThings}

【已发现的信息】
{hasFindThings}

【对话目标】
{target}

【玩家输入】
{playerInput}

【NPC/画外音回复】
{npcResponse}

【你的任务】

判断这次对话是否发现了【新的重要信息】。

判断标准：
- 信息必须不在""已发现的信息""中
- 信息必须在""已知重要信息列表""中
- 信息必须是具体的、可观察的

输出要求：
- 如果发现了新信息，返回新信息的描述
- 如果没有发现新信息，返回空字符串
- 只输出纯文本，不使用 JSON"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem
            .ChatToGPT(messages);

        if (!string.IsNullOrEmpty(result))
        {
            hasFindThings += $"\\n{result}";
        }
    }

    private async Task HandleRACommand(RACommand command, ChatInput input)
    {
        var rollSystem = GameFrameWork.Instance.rollSystem;
        if (rollSystem == null)
        {
            input.panel.AddMessage("【系统】骰子系统未初始化");
            return;
        }

        var results = new List<string>();
        var skillNameStr = string.IsNullOrEmpty(command.SkillName) ? "" : $"【{command.SkillName}】";

        for (int i = 0; i < command.Count; i++)
        {
            int roll = UnityEngine.Random.Range(1, 101);
            var result = rollSystem.ResolveResult(roll, command.TargetValue);
            
            var resultText = result switch
            {
                CocCheckResult.CriticalSuccess => "大成功!!",
                CocCheckResult.ExtremeSuccess => "极难成功",
                CocCheckResult.HardSuccess => "困难成功",
                CocCheckResult.Success => "成功",
                CocCheckResult.Failure => "失败",
                CocCheckResult.Fumble => "大失败..",
                _ => "失败"
            };

            var bonusStr = command.BonusType switch
            {
                BonusType.Bonus => " (奖励骰)",
                BonusType.Penalty => " (惩罚骰)",
                _ => ""
            };

            var countStr = command.Count > 1 ? $" [{i + 1}/{command.Count}]" : "";
            results.Add($"{skillNameStr}技能{command.TargetValue} | 投骰{roll}{bonusStr} → {resultText}{countStr}");
        }

        foreach (var r in results)
        {
            input.panel.AddMessage(r);
        }

        await Task.CompletedTask;
    }

    private async Task<ActionCheckResult> CheckPlayerAction(string playerInput, ChatInput input)
    {
        var needCheckResult = await AskGptIfNeedCheck(playerInput);
        
        if (!needCheckResult.needCheck)
        {
            return new ActionCheckResult { needCheck = false };
        }

        var skillName = needCheckResult.skillName;
        var requiredLevel = needCheckResult.requiredLevel;

        var player = GameFrameWork.Instance.playerManager.nowPlayer;
        if (player == null)
        {
            return new ActionCheckResult { needCheck = false };
        }

        var skillComponent = player.GetComponent<SkillComponent>();
        if (skillComponent == null)
        {
            return new ActionCheckResult { needCheck = false };
        }

        if (!System.Enum.TryParse<NpcSkill>(skillName, out var skill))
        {
            input.panel.AddMessage($"【系统】未找到技能: {skillName}");
            return new ActionCheckResult { needCheck = false };
        }

        var skillValue = skillComponent.GetNowSkill(skill);
        var roll = UnityEngine.Random.Range(1, 101);
        var rollResult = GameFrameWork.Instance.rollSystem.ResolveResult(roll, skillValue);
        
        var resultText = rollResult switch
        {
            CocCheckResult.CriticalSuccess => "大成功!!",
            CocCheckResult.ExtremeSuccess => "极难成功",
            CocCheckResult.HardSuccess => "困难成功",
            CocCheckResult.Success => "成功",
            CocCheckResult.Failure => "失败",
            CocCheckResult.Fumble => "大失败..",
            _ => "失败"
        };

        var checkInfo = $"技能检定:{skillName}({skillValue})|投骰:{roll}|{resultText}";
        
        input.panel.AddMessage(checkInfo);

        return new ActionCheckResult
        {
            needCheck = true,
            skillName = skillName,
            skillValue = skillValue,
            roll = roll,
            result = rollResult
        };
    }

    private class GptCheckResult
    {
        public bool needCheck;
        public string skillName;
        public string requiredLevel;
    }

    private class ActionCheckResult
    {
        public bool needCheck;
        public string skillName;
        public int skillValue;
        public int roll;
        public CocCheckResult result;
    }

    private string BuildOutcomePrompt(ActionCheckResult check)
    {
        if (!check.needCheck || check.result == null)
        {
            return "";
        }

        var resultDesc = check.result switch
        {
            CocCheckResult.CriticalSuccess => "大成功（ Extraordinary Success）- 超出预期的好结果",
            CocCheckResult.ExtremeSuccess => "极难成功（ Extreme Success）- 非常出色的表现",
            CocCheckResult.HardSuccess => "困难成功（ Hard Success）- 勉强但成功",
            CocCheckResult.Success => "普通成功（ Success）- 达到预期",
            CocCheckResult.Failure => "失败（ Failure）- 未达到要求",
            CocCheckResult.Fumble => "大失败（ Fumble）- 糟糕的结果，比预期更差",
            _ => "失败"
        };

        var outcomeGuidance = check.result switch
        {
            CocCheckResult.CriticalSuccess => "叙事结果：玩家表现完美，结果远超预期。你应该描述一个特别出色、超出玩家想象的好结果。",
            CocCheckResult.ExtremeSuccess => "叙事结果：玩家表现极其出色。你应该描述一个非常好的结果，玩家成功完成任务且有额外收获。",
            CocCheckResult.HardSuccess => "叙事结果：玩家勉强成功。你应该描述成功但有些勉强，可能需要付出一些小代价。",
            CocCheckResult.Success => "叙事结果：玩家正常成功。你应该描述符合预期的成功结果。",
            CocCheckResult.Failure => "叙事结果：玩家失败。你应该描述失败的结果，行动未能完成。",
            CocCheckResult.Fumble => "叙事结果：大失败。你应该描述一个糟糕的结果，可能带来负面后果或意外情况。",
            _ => "叙事结果：失败。"
        };

        return $@"

【技能检定结果】
技能: {check.skillName}
技能值: {check.skillValue}
投骰: {check.roll}
结果: {resultDesc}

【叙事指引】
{outcomeGuidance}

请根据上述检定结果生成叙事内容。";
    }

    private async Task<GptCheckResult> AskGptIfNeedCheck(string playerInput)
    {
        var skillList = string.Join(", ", new[]
        {
            "strength", "constitution", "size", "dexterity", "appearance", "intelligence", "power", "education",
            "luck", "sanity", "health", "spotHidden", "listen", "psychology", "occult", "cthulhuMythos",
            "archaeology", "history", "creditRating", "firstAid", "medicine", "mechanicalRepair", "electricalRepair",
            "electronics", "drive", "dodge", "persuade", "stealth", "brawl", "firearms", "fastTalk",
            "locksmith", "linguistics", "disguise", "animalTraining", "performance", "astronomy", "charm",
            "climb", "fineArt", "intimidate", "libraryUse", "psychoanalysis", "track", "throwing"
        });

        var messages = new List<QwenChatMessage>
        {
            new QwenChatMessage
            {
                role = "system",
                content = @"你是一个动作分析器，判断玩家的动作是否需要技能检定。"
            },
            new QwenChatMessage
            {
                role = "user",
                content = $@"【玩家动作】
{playerInput}

【可用技能列表】（必须使用以下英文技能名之一）:
{skillList}

请判断：
1. 是否需要技能检定？（简单动作如普通交谈、开门、走路不需要）
2. 需要什么技能？（必须从上方列表中选择，使用英文技能名）
3. 需要什么成功等级？（可选：Success, HardSuccess, ExtremeSuccess）

请用以下JSON格式返回：
{{""needCheck"": true/false, ""skillName"": ""技能名"", ""requiredLevel"": ""成功等级""}}"
            }
        };

        var result = await GameFrameWork.Instance.GptSystem.ChatToGPT<GptCheckResult>(messages);
        return result ?? new GptCheckResult { needCheck = false };
    }

    private ParsedInput ParseUserInput(string input)
    {
        var result = new ParsedInput();

        var match1 = Regex.Match(input, @"对([\\u4e00-\\u9fa5a-zA-Z0-9_]+)说[：:](.+)");
        if (match1.Success)
        {
            result.IsDialogue = true;
            result.TargetNpc = match1.Groups[1].Value;
            result.Message = match1.Groups[2].Value.Trim();
            return result;
        }

        var match2 = Regex.Match(input, @"^([\\u4e00-\\u9fa5a-zA-Z0-9_]+)[：:](.+)");
        if (match2.Success && sceneNpcs.ContainsKey(match2.Groups[1].Value))
        {
            result.IsDialogue = true;
            result.TargetNpc = match2.Groups[1].Value;
            result.Message = match2.Groups[2].Value.Trim();
            return result;
        }

        var match3 = Regex.Match(input, @"@([\\u4e00-\\u9fa5a-zA-Z0-9_]+)\\s+(.+)");
        if (match3.Success)
        {
            result.IsDialogue = true;
            result.TargetNpc = match3.Groups[1].Value;
            result.Message = match3.Groups[2].Value.Trim();
            return result;
        }

        result.IsDialogue = false;
        result.Message = input;
        return result;
    }
}

public class ParsedInput
{
    public bool IsDialogue;
    public string TargetNpc;
    public string Message;
}

public class KPStoryListener : IChatPanelListener
{
    private Func<ChatInput, Task> onSubmit;
    private Action onClose;
    
    public KPStoryListener(Func<ChatInput, Task> onSubmit, Action onClose)
    {
        this.onSubmit = onSubmit;
        this.onClose = onClose;
    }
    
    public async Task OnSubmit(ChatInput input)
    {
        await onSubmit?.Invoke(input);
    }

    public void OnClose()
    {
        onClose?.Invoke();
    }
}
