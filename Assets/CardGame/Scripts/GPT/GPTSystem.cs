using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Newtonsoft.Json;
public class QwenChatMessage
{
    public string role;  // "user" | "assistant" | "system"
    [TextArea(10, 10)]
    public string content;
}

public class QwenChatRequest
{
    public string model { get; set; } = "qwen-max";
    public object input { get; set; }  // 改为 object
}

public class QwenChatOutput
{
    public string finish_reason { get; set; }
    public string text { get; set; }
}

public class QwenChatResponse
{
    public QwenChatOutput output { get; set; }
    // usage/request_id 可选，按需添加
}

public class AIResponse
{
    public int choice { get; set; }
    public string answer { get; set; }
    public string reason { get; set; }
}

public class QwenChatClient
{
    private string _apiKey;
    private readonly HttpClient _httpClient;
    private readonly string _endpoint = "https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation";

    public QwenChatClient(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    /// <summary>
    /// 发送对话请求，传入上下文消息，返回AI回复
    /// </summary>
    /// <param name="messages">包括 system, user, assistant 多条消息组成的对话上下文</param>
    /// <returns>AI回复文本</returns>
    public async Task<string> SendChatAsync(IEnumerable<QwenChatMessage> messages)
    {
        var requestData = new QwenChatRequest
        {
            model = "qwen-turbo",
            input = new { messages = messages }
        };
        foreach(var x in messages)
        {
            Debug.Log(x.content);
        }
        var json = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_endpoint, content);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Debug.Log($"Qwen API Error: {response.StatusCode} - {responseString}");
            return "";
        }

        var chatResponse = JsonConvert.DeserializeObject<QwenChatResponse>(responseString);

        if (chatResponse?.output?.text != null)
        {
            return chatResponse.output.text.Trim();
        }
        return string.Empty;
    }
}


public class GptChatSession
{
    /// <summary>
    /// gpt要扮演的角色
    /// </summary>
    private string systemPrompt;
    /// <summary>
    /// 输出的约束
    /// </summary>
    public string constraint;
    /// <summary>
    /// 角色对话历史
    /// </summary>
    public List<string> chatHistory;
    // public List<QwenChatMessage> Messages { get; private set; }

    public GptChatSession(string systemPrompt)
    {
        this.systemPrompt = systemPrompt;
    }

    public void SetConstrain(string constraint)
    {
        this.constraint = constraint;
    }
    public void AddChatHistory(string name, string context)
    {
        chatHistory.Add(name+":"+context);
    }

    public List<QwenChatMessage> GenerateMessage()
    {
        var ret = new List<QwenChatMessage>();
        ret.Add(new QwenChatMessage
        {
            role = "system",
            content = systemPrompt
        });
        var strbuilder = new StringBuilder();
        foreach (var x in chatHistory)
        {
            strbuilder.Append(x);
            strbuilder.Append("\n");
        }
        ret.Add(new QwenChatMessage
        {
            role = "user",
            content = strbuilder.ToString()
        });
        ret.Add(new QwenChatMessage()
        {
            role = "system",
            content = constraint
        });
        return ret;
    }
}


public class GPTSystem:SerializedMonoBehaviour
{
    QwenChatClient gpt;
    public List<QwenChatMessage> messages;
    [Button]
    public void Chat()
    {
        gpt = new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");
        var responses = new List<QwenChatMessage>();
        responses.Add(new QwenChatMessage
        {
            role = "system",
            content = $"你要扮演一个npc用来与玩家交互,你扮演的npc性格是"
        });
        gpt.SendChatAsync(responses).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log(task.Result);
                // var res = JsonConvert.DeserializeObject<AIResponse>(task.Result);
            }
            else
            {
                Debug.LogError("Error: " + task.Exception.Message);
            }
        });
    }
    public async Task<T> ChatToGPT<T>(IEnumerable<QwenChatMessage> messages)
    {
        var gpt = new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");
        var ret = await gpt.SendChatAsync(messages).ContinueWith(task =>
        {
            if(task.IsCompleted)
            {
                var res = JsonConvert.DeserializeObject<T>(task.Result);
                return res;
            }
            else
            {
                return default(T);
            }
        });
        return ret;
    }
    /// <summary>
    /// 新增：多轮对话（有上下文记忆）
    /// </summary>
    public async Task<string> ChatInSession(GptChatSession session, string userInput)
    {
        // 1️⃣ 添加玩家输入
        session.AddUserMessage(userInput);

        // 2️⃣ 发送完整历史给 GPT
        var reply = await gpt.SendChatAsync(session.Messages);

        // 3️⃣ 记录 GPT 回复
        session.AddAssistantMessage(reply);

        return reply;
    }

    /// <summary>
    /// 多轮对话 + JSON 结构化返回（高级用法）
    /// </summary>
    public async Task<T> ChatInSession<T>(
        GptChatSession session,
        string userInput,
        string constrain,
        int maxRetry = 3
    )
    {
        session.AddUserMessage(userInput);
        session.AddSystemMessage(constrain);

        var gpt = new QwenChatClient("sk-fbe1e9616f8b4853b1bc54a79d25180f");

        string lastRaw = null;

        for (int attempt = 0; attempt < maxRetry; attempt++)
        {
            var raw = await gpt.SendChatAsync(session.Messages);
            lastRaw = raw;

            try
            {
                var res = JsonConvert.DeserializeObject<T>(raw);
                if (res != null)
                {
                    return res;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    $"[ChatInSession] JSON 解析失败，第 {attempt + 1} 次尝试\n{e.Message}\n原始输出:\n{raw}"
                );
            }

            // ❗关键：纠错提示，作为 system message 注入
            session.AddSystemMessage(
                $@"你刚才的输出 **不符合 JSON 格式要求**，无法被程序解析。

要求：
- 只输出合法 JSON
- 不要包含任何多余文本
- 不要解释
- 不要添加新信息
- 只修正格式问题

请严格重新输出。"
            );
        }

        throw new Exception(
            $"ChatInSession<{typeof(T).Name}> 在 {maxRetry} 次尝试后仍无法解析。\n最后一次原始输出：\n{lastRaw}"
        );
    }

}
public static class GptSchemaBuilder
{
    public static string BuildSchema(Type type, int indent = 0)
    {
        var sb = new StringBuilder();
        string pad = new string(' ', indent * 2);

        sb.AppendLine($"{pad}{{");

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++)
        {
            var f = fields[i];
            sb.Append($"{pad}  \"{f.Name}\": ");

            sb.Append(GetTypeDescription(f.FieldType, indent + 1));

            if (i < fields.Length - 1)
                sb.Append(",");

            sb.AppendLine();
        }

        sb.Append($"{pad}}}");
        return sb.ToString();
    }

    private static string GetTypeDescription(Type type, int indent)
    {
        if (type == typeof(string))
            return "\"string\"";

        if (type == typeof(int))
            return "number";

        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elemType = type.GetGenericArguments()[0];
            return $"[{BuildSchema(elemType, indent + 1)}]";
        }

        if (type.IsClass)
            return BuildSchema(type, indent + 1);

        return "\"unknown\"";
    }
}
