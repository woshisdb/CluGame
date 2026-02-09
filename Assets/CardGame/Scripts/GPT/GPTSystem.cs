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
    public static string apiKey = "sk-fbe1e9616f8b4853b1bc54a79d25180f";
    private readonly HttpClient _httpClient;
    private readonly string _endpoint = "https://dashscope.aliyuncs.com/api/v1/services/aigc/text-generation/generation";

    public QwenChatClient()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
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
            model = "qwen-plus-latest",
            input = new { messages = messages }
        };
        foreach(var x in messages)
        {
            Debug.Log(x.content);
        }
        var json = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.Timeout = TimeSpan.FromMinutes(10);
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
            var ret = chatResponse.output.text.Trim();
            return ret;
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
        chatHistory = new List<string>();
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
    public async Task<string> ChatToGPT(IEnumerable<QwenChatMessage> messages)
    {
        var gpt = new QwenChatClient();
        var ret = await gpt.SendChatAsync(messages);
        return ret;
    }
    public async Task<T> ChatToGPT<T>(
        IEnumerable<QwenChatMessage> messages,
        int times = 4
    )
    {
        Exception lastException = null;

        for (int i = 0; i < times; i++)
        {
            try
            {
                var gpt = new QwenChatClient();

                // ⚠️ 建议在 SendChatAsync 内部支持 CancellationToken
                var resultText = await gpt.SendChatAsync(messages);

                if (string.IsNullOrWhiteSpace(resultText))
                {
                    throw new Exception("GPT 返回空内容");
                }
                
                // // ⚠️ 防止 GPT 偶尔输出非 JSON（你已经遇到过）
                // if (!resultText.TrimStart().StartsWith("{"))
                // {
                //     throw new Exception($"GPT 返回非 JSON 内容：{resultText.Substring(0, Math.Min(30, resultText.Length))}");
                // }

                var result = JsonConvert.DeserializeObject<T>(resultText);
                return result;
            }
            catch (Exception e)
            {
                lastException = e;
                Debug.LogWarning($"GPT 第 {i + 1}/{times} 次失败：{e.Message}");

                // 简单退避，避免立刻重试
                await Task.Delay(500 + i * 300);
            }
        }

        Debug.LogError($"GPT 请求 {times} 次全部失败，跳过该段。最后错误：{lastException?.Message}");
        return default(T);
    }

    /// <summary>
    /// 新增：多轮对话（有上下文记忆）
    /// </summary>
    public async Task<string> ChatInSession(GptChatSession session, string userInput)
    {
        // 使用三段式对话：1) 系统规则 2) 对话历史 3) 输出约束
        // 将用户输入记录到历史，不修改主 Messages
        session.AddChatHistory("Player", userInput);
        var messages = session.GenerateMessage();
        var reply = await gpt.SendChatAsync(messages);
        // 将 GPT 回复记录到历史
        session.AddChatHistory("NPC", reply);
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
        // 使用三段式对话：1) 系统规则 2) 对话历史 3) 输出约束
        session.AddChatHistory("Player", userInput);
        session.SetConstrain(constrain);
        var gpt = new QwenChatClient();
        string lastRaw = null;

        for (int attempt = 0; attempt < maxRetry; attempt++)
        {
            var messages = session.GenerateMessage();
            var raw = await gpt.SendChatAsync(messages);
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
