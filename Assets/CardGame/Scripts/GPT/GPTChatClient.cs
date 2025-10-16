//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;
//using Sirenix.OdinInspector;
//using Sirenix.Serialization;
//using UnityEngine;
//using Newtonsoft.Json;
//public class QwenChatMessage
//{
//    public string role;  // "user" | "assistant" | "system"
//    [TextArea(10, 10)]
//    public string content;
//}

//public class QwenChatRequest
//{
//    public string model { get; set; } = "qwen-max";
//    public object input { get; set; }  // 改为 object
//}

//public class QwenChatOutput
//{
//    public string finish_reason { get; set; }
//    public string text { get; set; }
//}

//public class QwenChatResponse
//{
//    public QwenChatOutput output { get; set; }
//    // usage/request_id 可选，按需添加
//}

//public class AIResponse
//{
//    public int choice { get; set; }
//    public string answer { get; set; }
//    public string reason { get; set; }
//}

//public class QwenChatClient
//{
//    private string _apiKey;
//    private readonly HttpClient _httpClient;
//    private readonly string _endpoint = "https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation";

//    public QwenChatClient(string apiKey)
//    {
//        _apiKey = apiKey;
//        _httpClient = new HttpClient();
//        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
//    }

//    /// <summary>
//    /// 发送对话请求，传入上下文消息，返回AI回复
//    /// </summary>
//    /// <param name="messages">包括 system, user, assistant 多条消息组成的对话上下文</param>
//    /// <returns>AI回复文本</returns>
//    public async Task<string> SendChatAsync(List<QwenChatMessage> messages)
//    {
//        var requestData = new QwenChatRequest
//        {
//            model = "qwen-turbo",
//            input = new { messages = messages }
//        };
//        foreach(var x in messages)
//        {
//            Debug.Log(x.content);
//        }
//        var json = JsonConvert.SerializeObject(requestData);
//        var content = new StringContent(json, Encoding.UTF8, "application/json");

//        var response = await _httpClient.PostAsync(_endpoint, content);
//        var responseString = await response.Content.ReadAsStringAsync();

//        if (!response.IsSuccessStatusCode)
//        {
//            Debug.Log($"Qwen API Error: {response.StatusCode} - {responseString}");
//            return "";
//        }

//        var chatResponse = JsonConvert.DeserializeObject<QwenChatResponse>(responseString);

//        if (chatResponse?.output?.text != null)
//        {
//            return chatResponse.output.text.Trim();
//        }

//        return string.Empty;
//    }
//}



//public class GPTChatClient:SerializedMonoBehaviour
//{
//    QwenChatClient gpt;
//    public INpc npc;
//    public INpc player;
//    public List<QwenChatMessage> messages;
//    void Start()
//    {
//        // 假设你已经将 QwenChatClient 挂载到一个 GameObject 上
//        //gpt =new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");


//        //gpt.SendChatAsync(messages).ContinueWith(task =>
//        //{
//        //    if (task.IsCompletedSuccessfully)
//        //    {
//        //        Debug.Log("Qwen 回复: " + task.Result);
//        //    }
//        //    else
//        //    {
//        //        Debug.LogError("Error: " + task.Exception.Message);
//        //    }
//        //});
//    }
//    [Button]
//    public void Chat()
//    {
//        var npcMood = NpcGptUtils.GetPersonalityDescription(((Npc)npc).npcPersonality);
//        Debug.Log(npcMood);
//        gpt = new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");
//        //messages.Add(new QwenChatMessage
//        //{
//        //    role = "system",
//        //    content = $"你要扮演一个npc用来与玩家交互,你扮演的npc性格是:{npcMood}"
//        //});
//        var responses = new List<QwenChatMessage>();
//        responses.Add(new QwenChatMessage
//        {
//            role = "system",
//            content = $"你要扮演一个npc用来与玩家交互,你扮演的npc性格是:{npcMood}"
//        });
//        responses.Add(new QwenChatMessage
//        {
//            role = "system",
//            content = $"你的特性是:{npc.GetINpcPropertyStr()}"
//        });
//        responses.Add(new QwenChatMessage
//        {
//            role = "system",
//            content = $"你和玩家的过往有:{npc.GetMemoryStr(player)}"
//        });
//        foreach (var message in messages)
//        {
//            responses.Add(message);
//        }
//        gpt.SendChatAsync(responses).ContinueWith(task =>
//        {
//            if (task.IsCompleted)
//            {
//                var res = JsonConvert.DeserializeObject<AIResponse>(task.Result);
//                Debug.Log("Qwen 选择: " + res.choice);
//                Debug.Log("Qwen 回复: " + res.answer);
//                Debug.Log("Qwen 理由: " + res.reason);
//            }
//            else
//            {
//                Debug.LogError("Error: " + task.Exception.Message);
//            }
//        });
//    }
//    public static async Task<T> ChatToGPT<T>(List<QwenChatMessage> messages)
//    {
//        var gpt = new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");
//        var ret = await gpt.SendChatAsync(messages).ContinueWith(task =>
//        {
//            if(task.IsCompleted)
//            {
//                var res = JsonConvert.DeserializeObject<T>(task.Result);
//                return res;
//            }
//            else
//            {
//                return default(T);
//            }
//        });
//        return ret;
//    }

//    [Button]
//    public void GetResult(string toDo)
//    {
//        var gpt = new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");
//        var responses = new List<QwenChatMessage>();
//        responses.Add(new QwenChatMessage
//        {
//            role = "system",
//            content = $@"玩家想做的事情为: {toDo}。
//请结合现实情况按照CoC跑团七版规则判断是否需要技能检定。

//如果行为非常简单或完全不可能完成，请直接返回 true 或 false，不需要检定；
//如果行为有一定难度或不确定性，则返回true。

//特别注意：CheckType 字段必须使用 AnimalProperty.技能名称 的格式，例如 AnimalProperty.Stealth。

//请严格按照以下 JSON 格式返回响应：

//{{
//    ""NeedCheck"": true,
//    ""CheckType"": AnimalProperty.技能名称,
//    ""Result"": true/false,
//    ""Reason"": ""简要说明判断理由""
//    ""npcResist"": ""true/false,表示是否由NPC进行检定来对抗玩家的行为，而不是玩家自己进行检定。""
//}}

//示例：
//如果玩家行为是 ""开枪射击敌人""，返回：
//{{
//    ""NeedCheck"": true,
//    ""CheckType"": ""AnimalProperty.GunSkill"",
//    ""Result"": true,
//    ""Reason"": ""需要使用枪械技能进行检定""
//    ""npcResist"": ""false""
//}}
//public enum AnimalProperty
//{{
//    Brave,
//    Appearance,
//    Constitution,
//    Charm,
//    Agility,
//    Strength,
//    Intelligence,
//    Education,
//    Literature,
//    Math,
//    CombatSkill,
//    MedicalSkill,
//    ArchitectureSkill,
//    BusinessSkill,
//    TeachingSkill,
//    CookingSkill,
//    CraftingSkill,
//    TacticsSkill,
//    Influence,
//    Reputation,
//    Willpower,
//    Archaeology,
//    Accounting,
//    Astronomy,
//    SpotHidden,
//    Track,
//    Law,
//    Psychology,
//    Disguise,
//    Persuade,
//    Intimidate,
//    FirstAid,
//    Perform,
//    Stealth,
//    GunSkill,
//}}
//"
//        });

//        gpt.SendChatAsync(responses).ContinueWith(task =>
//        {
//            if (task.IsCompleted)
//            {
//                Debug.Log(task.Result);
//                var res = JsonConvert.DeserializeObject<GptPlayerCkeckTask>(task.Result);
//                Debug.Log("Qwen 结果: " + res.Result);
//                Debug.Log("Qwen 需要检测吗: " + res.NeedCheck);
//                Debug.Log("Qwen 检测类型: " + res.CheckType);
//                Debug.Log("Qwen 检测类型: " + res.Reason);
//                Debug.Log("Qwen 检测类型: " + res.npcResist);
//                var result = res.Check(npc);
//                Debug.Log(result.ToString());
//                return res;
//            }
//            else
//            {
//                return default(GptPlayerCkeckTask);
//            }
//        });

//    }
//}