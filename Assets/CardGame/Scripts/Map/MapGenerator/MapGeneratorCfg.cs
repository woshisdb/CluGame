using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class MapItemCfg
{
    public GameObject view;
    public CellViewType CellViewType;
}

/// <summary>
/// 地图生成的配置
/// </summary>
public class MapGeneratorCfg:SerializedScriptableObject
{
    public int[,] grid = new int[5,5];
    public int[,] wallx = new int[5,6];
    public int[,] wally = new int[6,5];
    /// <summary>
    /// 地图配置
    /// </summary>
    public List<MapItemCfg> MapItemCfgs;

    public List<MapItemCfg> WallItemCfgs;
    [TableMatrix(DrawElementMethod = "DrawElement", SquareCells = true)]
    public List<List<CellModel>> CellModels;
    
    [Button]
    public void Generate()
    {
        
    }
    
#if UNITY_EDITOR
    // 该函数由 Odin 调用，用于绘制每个单元格
    private static CellModel DrawElement(Rect rect, CellModel value)
    {
        // 根据是否是墙改变颜色
        Color col = Color.white;
        if (value== null)
        {
            col = Color.black;
        }
        // 绘制背景
        EditorGUI.DrawRect(rect.Padding(1), col);

        // 返回更新后的值
        return value;
    }
#endif
}