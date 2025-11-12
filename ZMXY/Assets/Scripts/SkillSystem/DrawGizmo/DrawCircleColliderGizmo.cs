using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircleColliderGizmo : MonoBehaviour
{
    [Header("检测设置")]
    public float angle; // 扇形角度
    public float minRadius;
    public float maxRadius;
    
    [Header("可视化设置")]
    public bool showSector = true;
    public Color sectorColor = new Color(1, 1, 1, 1f);
    public Color sectorBorderColor = Color.white;
    public Color ringColor = new Color(1, 1, 1, 1); // 环形区域颜色
    
    
    void OnDrawGizmos()
    {
        if (!showSector) return;

        DrawSectorGizmo();
        
        // if (showDetectedTargets)
        // {
        //     DrawDetectedTargets();
        // }
    }
    
    void DrawSectorGizmo()
    {
        // 绘制扇形填充区域
        //DrawSectorFill();

        // 绘制扇形边界
        DrawSectorBorder();

        // 绘制检测半径
        DrawRadiusCircle();
            
        // 绘制环形扇形区域（3-5范围）
        DrawRingSectorFill();
    }
    
    void DrawRingSectorFill()
    {
        Gizmos.color = ringColor;

        float halfAngle = angle / 2f;
        int segments = Mathf.Max(24, Mathf.RoundToInt(angle / 3f)); // 更多分段以获得更好的效果

        Vector2 center = transform.position;
        Vector2 forward = GetForwardDirection();

        // 创建内外弧的点
        Vector3[] innerVertices = new Vector3[segments + 1];
        Vector3[] outerVertices = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (angle / segments) * i;
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * forward;
        
            innerVertices[i] = center + direction * minRadius;
            outerVertices[i] = center + direction * maxRadius;
        }

        // 绘制环形扇形的四边形
        for (int i = 0; i < segments; i++)
        {
            // 绘制四边形（两个三角形）
            Gizmos.DrawLine(innerVertices[i], innerVertices[i + 1]);
            Gizmos.DrawLine(outerVertices[i], outerVertices[i + 1]);
            Gizmos.DrawLine(innerVertices[i], outerVertices[i]);
            Gizmos.DrawLine(innerVertices[i + 1], outerVertices[i + 1]);
        }
    }
    
    /// <summary>
    /// 角色朝向
    /// </summary>
    /// <returns></returns>
    Vector2 GetForwardDirection()
    {
        // 根据你的游戏设置选择前方方向
        // 对于2D角色，通常是transform.right（如果面朝右边）
        // 或者transform.up（如果面朝上方）
        return transform.right;
    }
    
     void DrawSectorFill()
    {
        Gizmos.color = sectorColor;

        float halfAngle = angle / 2f;
        int segments = Mathf.Max(12, Mathf.RoundToInt(angle / 5f)); // 动态分段

        Vector2 center = transform.position;
        Vector2 forward = GetForwardDirection();

        // 创建扇形网格点
        Vector3[] vertices = new Vector3[segments + 2];
        vertices[0] = center;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (angle / segments) * i;
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * forward;
            vertices[i + 1] = center + direction * maxRadius;
        }

        // 绘制三角形（使用Gizmos.DrawLine模拟填充）
        for (int i = 0; i < segments; i++)
        {
            Gizmos.DrawLine(vertices[0], vertices[i + 1]);
            Gizmos.DrawLine(vertices[i + 1], vertices[i + 2]);
        }
    }

    void DrawSectorBorder()
    {
        Gizmos.color = sectorBorderColor;

        float halfAngle = angle / 2f;
        int segments = 24;
        Vector2 center = transform.position;
        Vector2 forward = GetForwardDirection();

        // 绘制边界线
        Vector2 startDir = Quaternion.Euler(0, 0, -halfAngle) * forward;
        Vector2 endDir = Quaternion.Euler(0, 0, halfAngle) * forward;

        Gizmos.DrawLine(center, center + startDir * maxRadius);
        Gizmos.DrawLine(center, center + endDir * maxRadius);

        // 绘制弧线
        Vector2 prevPoint = center + startDir * maxRadius;
        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (angle / segments) * i;
            Vector2 currentDir = Quaternion.Euler(0, 0, currentAngle) * forward;
            Vector2 currentPoint = center + currentDir * maxRadius;

            Gizmos.DrawLine(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }
    }

    void DrawRadiusCircle()
    {
        Gizmos.color = new Color(1, 1, 1, 0.2f);
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }

    // void DrawDetectedTargets()
    // {
    //     Gizmos.color = targetColor;
    //
    //     foreach (var collider in detectedColliders)
    //     {
    //         if (collider != null)
    //         {
    //             // 绘制目标位置
    //             Gizmos.DrawWireSphere(collider.transform.position, 0.3f);
    //             
    //             // 绘制到目标的连线
    //             Gizmos.DrawLine(transform.position, collider.transform.position);
    //
    //             // 如果检测了视线，绘制视线检测结果
    //             if (obstacleLayer != 0)
    //             {
    //                 bool hasLOS = HasLineOfSight(collider.transform);
    //                 Gizmos.color = hasLOS ? Color.green : Color.red;
    //                 Gizmos.DrawWireSphere(collider.transform.position, 0.4f);
    //                 Gizmos.color = targetColor;
    //             }
    //         }
    //     }
    // }

}
