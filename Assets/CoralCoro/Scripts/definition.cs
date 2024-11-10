using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
[System.Serializable]
public class MusicBlock
{
    public float starttime;
    public float lifespan;
    public int gridYaw; // Yaw 网格索引（0-5），对应 -90 到 90
    public int gridPitch; // Pitch 网格索引（0-2），对应 -60 到 60
    public int type;
    public int pitch;
    public static Vector3 origin = new Vector3(0, 1.5f, 0); // Current eyesight level
    public static float radius = 0.60f; // 方块所在的球面

    public Vector3 GetPosition()
    {
        float cellYaw = 30.0f; // 单位方块的Yaw大小
        float cellPitch = 30.0f; // 单位方块的Pitch大小
        // 计算具体的pitch和yaw角度
        float yaw = gridYaw * cellYaw - 75.0f; // -75, -45, -15, 0, 15, 45, 75
        float pitch = gridPitch * cellPitch - 30.0f; // -30, 0, 30

        // 将球面坐标（pitch, yaw）转换为笛卡尔坐标
        float radPitch = pitch * Mathf.Deg2Rad; // 转换为弧度
        float radYaw = yaw * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(radPitch) * Mathf.Sin(radYaw);
        float y = radius * Mathf.Sin(radPitch);
        float z = radius * Mathf.Cos(radPitch) * Mathf.Cos(radYaw);

        Vector3 position = origin + new Vector3(x, y, z);
        return position;
    }

    public Vector3 GetFeedbackPosition()
    {
        float cellYaw = 30.0f; // 单位方块的Yaw大小
        float cellPitch = 30.0f; // 单位方块的Pitch大小
        // 计算具体的pitch和yaw角度
        float yaw = gridYaw * cellYaw - 75.0f; // -75, -45, -15, 0, 15, 45, 75
        float pitch = gridPitch * cellPitch - 30.0f; // -30, 0, 30

        // feedback position
        yaw += 0.0f;
        pitch += 7.0f;

        // 将球面坐标（pitch, yaw）转换为笛卡尔坐标
        float radPitch = pitch * Mathf.Deg2Rad; // 转换为弧度
        float radYaw = yaw * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(radPitch) * Mathf.Sin(radYaw);
        float y = radius * Mathf.Sin(radPitch);
        float z = radius * Mathf.Cos(radPitch) * Mathf.Cos(radYaw);

        Vector3 position = origin + new Vector3(x, y, z);
        return position;
    }

    public Quaternion GetRotation(Vector3 position)
    {
        Vector3 direction = position - origin; // 从原点到方块位置的方向
        return Quaternion.LookRotation(direction); // 计算旋转
    }
}
