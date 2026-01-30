using Sirenix.OdinInspector;

public enum MovementForm
{
    [LabelText("地面行走（步行/奔跑）")]
    Ground,

    [LabelText("爬行 / 滑行")]
    Crawling,

    [LabelText("攀爬（可在墙壁或天花板移动）")]
    Climbing,

    [LabelText("飞行 / 悬浮")]
    Flying,

    [LabelText("游泳（水中移动）")]
    Swimming,

    [LabelText("钻地（地下穿行）")]
    Burrowing,

    [LabelText("相位 / 穿越（穿墙或传送）")]
    Phasing
}

