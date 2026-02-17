using System.Collections.Generic;

public class WorldMapSystem
{
    /// <summary>
    /// 一系列的场景,--------
    /// </summary>
    public List<SpaceCardModel> Spaces
    {
        get
        {
            return GameFrameWork.Instance.data.saveFile.Space;
        }
    }
}