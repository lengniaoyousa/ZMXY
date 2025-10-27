using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoxColliderGizmo : MonoBehaviour
{
    [Header("BoxCollider 参数")]
    public Vector3 size = new Vector3(1f, 1f, 1f);    // 大小
    public Vector3 center = new Vector3(0f, 0.5f, 0f); // 偏移（中心点）
    public Vector3 rotation = new Vector3(0f, 0f, 0f);//旋转
    
    [Header("绘制设置")]
    public Color wireColor = Color.red;
    public Color solidColor = new Color(0f, 1f, 0f, 0.3f);
    public bool drawSolid = true;
    public bool drawWire = true;

    void OnDrawGizmos()
    {
        // 保存原来的矩阵
        Matrix4x4 originalMatrix = Gizmos.matrix;
        
        // 设置新的矩阵（考虑物体的位置、旋转和缩放）
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + Quaternion.Euler(rotation) * center, // 世界坐标位置 + 偏移
            transform.rotation,                               // 物体的旋转
            transform.lossyScale                              // 世界坐标的缩放
        );

        // 绘制实心盒子
        if (drawSolid)
        {
            Gizmos.color = solidColor;
            Gizmos.DrawCube(Vector3.zero, size);
        }

        // 绘制线框盒子
        if (drawWire)
        {
            Gizmos.color = wireColor;
            Gizmos.DrawWireCube(Vector3.zero, size);
        }

        // 恢复原来的矩阵
        Gizmos.matrix = originalMatrix;
        
        // 绘制中心点
        DrawCenterPoint();
    }

    void DrawCenterPoint()
    {
        Vector3 worldCenter = transform.position + transform.rotation * center;
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldCenter, 0.1f);
        
        // 绘制从物体中心到Box中心的连线
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, worldCenter);
    }
}
