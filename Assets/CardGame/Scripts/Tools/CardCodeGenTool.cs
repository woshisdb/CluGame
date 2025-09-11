using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class CardCodeGenTool : MonoBehaviour
{
    public string path;
    public string Name;
    public string title;
    public string description;
    public string FlagInterface;
    public CardEnum cardEnum;
    [Button]
    public void GenCode()
    {
        // 生成数据类代码
        string dataClassCode = GenerateDataClass(Name, title, description, FlagInterface, cardEnum);
        // 生成模型类代码
        string modelClassCode = GenerateModelClass(Name);

        // 确保输出目录存在
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 写入文件
        File.WriteAllText(Path.Combine(path, $"{Name}CardData.cs"), dataClassCode+"\n\n"+modelClassCode);
    }
    /// <summary>
    /// 生成数据类代码
    /// </summary>
    private string GenerateDataClass(string CardName, string CardTitle, string CardDescription,string FlagInterface,CardEnum cardEnum)
    {
        return $@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class {CardName}CardData : CardData, {FlagInterface}
{{
    public {CardName}CardData() : base()
    {{
        title = ""{CardTitle.ToLower()}"";
        description = ""{CardDescription}"";
        InitCardFlags(typeof({FlagInterface}));
    }}

    public override CardModel CreateModel()
    {{
        return new {CardName}CardModel(this);
    }}

    public override CardEnum GetCardType()
    {{
        return CardEnum.{cardEnum.ToString()};
    }}
}}";
    }

    /// <summary>
    /// 生成模型类代码
    /// </summary>
    private string GenerateModelClass(string CardName)
    {
        return $@"

public class {CardName}CardModel : CardModel
{{
    public {CardName}CardModel(CardData cardData) : base(cardData)
    {{
        // 模型初始化逻辑
    }}
}}";
    }
}
