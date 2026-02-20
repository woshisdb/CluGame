using System;
using System.Text.RegularExpressions;

public enum BonusType
{
    None,
    Bonus,      // 奖励骰
    Penalty     // 惩罚骰
}

public class RACommand
{
    public string SkillName;   // 技能名（可为空）
    public int TargetValue;    // 技能值
    public int Count = 1;      // 次数
    public BonusType BonusType = BonusType.None;
}

public static class RAParser
{
    public static bool TryParse(string input, out RACommand command)
    {
        command = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim();

        // 必须以 .ra 开头
        if (!input.StartsWith(".ra", StringComparison.OrdinalIgnoreCase))
            return false;

        // 去掉 ".ra"
        string content = input.Substring(3).Trim();

        if (string.IsNullOrEmpty(content))
            return false;

        var tokens = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        command = new RACommand();

        int index = 0;

        // 判断奖励 / 惩罚骰
        if (tokens[index].Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            command.BonusType = BonusType.Bonus;
            index++;
        }
        else if (tokens[index].Equals("p", StringComparison.OrdinalIgnoreCase))
        {
            command.BonusType = BonusType.Penalty;
            index++;
        }

        if (index >= tokens.Length)
            return false;

        // 如果当前是数字 → 无技能名
        if (int.TryParse(tokens[index], out int target))
        {
            command.TargetValue = target;
            index++;
        }
        else
        {
            // 当前是技能名
            command.SkillName = tokens[index];
            index++;

            if (index >= tokens.Length)
                return false;

            if (!int.TryParse(tokens[index], out target))
                return false;

            command.TargetValue = target;
            index++;
        }

        // 解析次数（可选）
        if (index < tokens.Length)
        {
            if (int.TryParse(tokens[index], out int count))
            {
                command.Count = Math.Max(1, count);
            }
        }

        return true;
    }
}