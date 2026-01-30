using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Laser360 : SerializedMonoBehaviour
{
    public int rayCount = 360;       // 射线数量 = 每度一条
    public float radius = 20f;       // 最大距离
    public float height = 1.6f;      // 眼睛高度
    public LayerMask obstacleMask;   // 墙体层
    public HashSet<CellSeeContainer> seeCells;
    void Update()
    {
        foreach (var obj in seeCells)
        {
            obj.allRay = 0;
            obj.seeRay = 0;
        }
        Vector3 origin = transform.position + Vector3.up * height;

        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;

            // 将角度转为方向
            Vector3 dir = AngleToDir(angle);
            RaycastHit[] hits = Physics.SphereCastAll(
                origin,
                0.01f,      // 很小的半径，也能命中内部
                dir,
                radius,
                obstacleMask
            );
            if (hits.Length > 0)
            {
                // 按距离排序，取最近命中点（RaycastAll 不保证顺序）
                Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
                // 画红线到最近障碍
                float startRay = 1;
                bool hasDraw = false;
                for (int j = 0; j < hits.Length; j++)
                {
                    var hit = hits[j];
                    var comp = hit.transform.GetComponent<CellSeeContainer>();//获取
                    if (!seeCells.Contains(comp))
                    {
                        seeCells.Add(comp);
                    }
                    comp.allRay += 1;
                    comp.seeRay += startRay;
                    startRay *= comp.cellSeeRate;
                    if (startRay<=0&&!hasDraw)
                    {
                        Debug.DrawLine(origin, hit.point, Color.red, 0f, false);
                        hasDraw = true;
                    }
                }

                if (!hasDraw)
                {
                    Debug.DrawLine(origin, origin + dir * radius, Color.green, 0f, false);
                }
            }
            else
            {
                // 没撞到任何物体，画绿线
                Debug.DrawLine(origin, origin + dir * radius, Color.green, 0f, false);
            }
        }
    }

    // 把角度（度）转换成水平平面的方向
    Vector3 AngleToDir(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
    }
}